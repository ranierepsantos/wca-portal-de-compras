using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Solicitacoes.Common;
using wca.share.application.Features.Solicitacoes.Common.Data.Extensions;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Solicitacoes.Queries
{
    public record SolicitacaoPaginateQuery(
        DateTime? DataIni, 
        DateTime? DataFim, 
        int? ResponsavelId, 
        int[]? ClienteIds,
        int[]? CentroCustoIds,
        int[]? Status,
        EnumTipoSolicitacao TipoSolicitacao = EnumTipoSolicitacao.Todos) : PaginationQuery, IRequest<ErrorOr<Pagination<SolicitacaoToPaginateResponse>>>;
    internal sealed class SolicitacaoToPaginateQueryHandle :
        IRequestHandler<SolicitacaoPaginateQuery, ErrorOr<Pagination<SolicitacaoToPaginateResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SolicitacaoToPaginateQueryHandle> _logger;
        public SolicitacaoToPaginateQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<SolicitacaoToPaginateQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<SolicitacaoToPaginateResponse>>> Handle(
            SolicitacaoPaginateQuery request, CancellationToken cancellationToken)
        {

            try
            {
                if (request.DataIni > request.DataFim || (request.DataIni != null && request.DataFim is null) || (request.DataFim != null && request.DataIni is null))
                {
                    return Error.Validation("Data início ou fim inválida!");
                }

                IQueryable<Solicitacao> query;

                if (request.TipoSolicitacao == EnumTipoSolicitacao.Todos && request.CentroCustoIds.Length > 0)
                {
                    string condicao = String.Join(",", request.CentroCustoIds);

                    string consulta = @$"
                                    select s.id, s.solicitacaotipo_id, s.cliente_id, s.responsavel_id, s.data_solicitacao, s.status_id, s.descricao
                                    from solicitacoes s
                                    inner join clientes c on c.id = s.cliente_id
                                    left  join Usuarios u on u.id = s.responsavel_id
                                    left  join SolicitacaoComunicado sc on sc.solicitacao_id = s.id  and sc.centrocusto_id in ({condicao})
                                    left  join SolicitacaoDesligamento sd on sd.solicitacao_id = s.id  and sd.centrocusto_id in ({condicao})
                                    left  join SolicitacaoFerias sf on sf.solicitacao_id = s.id  and sf.centrocusto_id in ({condicao})
                                    left  join SolicitacaoMudancaBase sm on sm.solicitacao_id = s.id  and sm.centrocusto_id in ({condicao})
                                    where solicitacaotipo_id = 5
                                       or (solicitacaotipo_id =1 and sd.centrocusto_id is not null) 
                                       or (solicitacaotipo_id =2 and sc.centrocusto_id is not null)
                                       or (solicitacaotipo_id =3 and sf.centrocusto_id is not null)
                                       or (solicitacaotipo_id =4 and sm.centrocusto_id is not null)
                                    ";
                    query = _repository.FromQuery<Solicitacao>(consulta);
                }else
                {
                    query = _repository.GetDbSet<Solicitacao>();
                }

                query = query.IncludeCliente()
                             .IncludeResponsavel()
                             .IncludeHistorico()
                             .IncludeTipo()
                             .IncludeStatus(); 
                
                if (request.FilialId > 1)
                    query = query.Where(q => q.Cliente.FilialId.Equals(request.FilialId));

                if (request.ResponsavelId > 0)
                    query = query.Where(q => q.ResponsavelId.Equals(request.ResponsavelId));

                if (request.ClienteIds?.Length > 0)
                    query = query.Where(q => request.ClienteIds.Contains(q.ClienteId));

                if (request.Status?.Length > 0)
                    query = query.Where(q => request.Status.Contains(q.StatusSolicitacaoId));

                if (request.TipoSolicitacao != EnumTipoSolicitacao.Todos)
                    query = query.Where(q => q.SolicitacaoTipoId.Equals((int)request.TipoSolicitacao));

                if (request.DataIni != null && request.DataFim != null)
                {
                    var dataFim = request.DataFim.Value.AddHours(23).AddMinutes(59);
                    query = query.Where(c => c.DataSolicitacao >= request.DataIni && c.DataSolicitacao <= dataFim);
                }


                if (request.TipoSolicitacao == EnumTipoSolicitacao.Comunicado)
                {
                    query = query.IncludeComunicado();
                    if (request.CentroCustoIds?.Length > 0)
                        query = query.Where(q => request.CentroCustoIds.Contains(q.Comunicado.CentroCustoId));
                }
                else if (request.TipoSolicitacao == EnumTipoSolicitacao.Desligamento)
                {
                    query = query.IncludeDesligamento();
                    if (request.CentroCustoIds?.Length > 0)
                        query = query.Where(q => request.CentroCustoIds.Contains(q.Desligamento.CentroCustoId));

                } else if (request.TipoSolicitacao == EnumTipoSolicitacao.Ferias)
                {
                    query = query.IncludeFerias();
                    if (request.CentroCustoIds?.Length > 0)
                        query = query.Where(q => request.CentroCustoIds.Contains(q.Ferias.CentroCustoId));

                }
                else if (request.TipoSolicitacao == EnumTipoSolicitacao.MudancaBase)
                {
                    query = query.IncludeMudancaBase();
                    if (request.CentroCustoIds?.Length > 0)
                        query = query.Where(q => request.CentroCustoIds.Contains(q.MudancaBase.CentroCustoId));
                }
                else if (request.TipoSolicitacao == EnumTipoSolicitacao.Vaga)
                {
                    query = query.IncludeVagaToPaginate();
                }
                else
                {
                    query = query.IncludeComunicado()
                        .IncludeDesligamento()
                        .IncludeFerias()
                        .IncludeMudancaBase()
                        .IncludeVagaToPaginate();
                }

                query = query.OrderByDescending(q => q.DataSolicitacao);

                var pagination = Pagination<SolicitacaoToPaginateResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

                return await Task.FromResult(pagination);

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Parâmetros: {JsonSerializer.Serialize(request)}");
                _logger.LogError($"Error.Message: {ex.Message}");
                _logger.LogError($"Error.InnerException: {ex.InnerException?.Message}");
                return Error.Failure(ex.Message);
            }
        }
    }
}
