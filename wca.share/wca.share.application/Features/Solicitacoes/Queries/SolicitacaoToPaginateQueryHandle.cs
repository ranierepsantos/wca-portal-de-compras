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
        int UsuarioId = 0,
        int? FuncionarioId = 0,
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

                IQueryable<Solicitacao> query = _repository.GetDbSet<Solicitacao>();

                if (request.TipoSolicitacao == EnumTipoSolicitacao.Todos)
                {
                    string condicao = "";
                    if (request.UsuarioId > 0)
                        condicao = $" and ucc.usuarioid = {request.UsuarioId}"; 

                    string consulta = @$"
                                    select s.id, s.solicitacaotipo_id, s.cliente_id, s.responsavel_id, s.data_solicitacao, s.status_id, 
                                           s.descricao, s.criado_por, supervisor_id
                                    from solicitacoes s
                                    inner join clientes c on c.id = s.cliente_id
                                    left  join Usuarios u on u.id = s.responsavel_id
                                    left  join (select solicitacao_id from SolicitacaoComunicado sc inner join UsuarioCentrodeCustos ucc on ucc.CentroCustoId = sc.centrocusto_id {condicao} ) sc on sc.solicitacao_id = s.id  
                                    left  join (select solicitacao_id from SolicitacaoDesligamento sd inner join UsuarioCentrodeCustos ucc on ucc.CentroCustoId = sd.centrocusto_id {condicao}) sd on sd.solicitacao_id = s.id
                                    left  join (select solicitacao_id from SolicitacaoFerias sf inner join UsuarioCentrodeCustos ucc on ucc.CentroCustoId = sf.centrocusto_id {condicao}) sf on sf.solicitacao_id = s.id
                                    left  join (select solicitacao_id from SolicitacaoMudancaBase sm inner join UsuarioCentrodeCustos ucc on ucc.CentroCustoId = sm.centrocusto_id {condicao}) sm on sm.solicitacao_id = s.id
                                    where solicitacaotipo_id = 5
                                       or (solicitacaotipo_id =1 and sd.solicitacao_id is not null) 
                                       or (solicitacaotipo_id =2 and sc.solicitacao_id is not null)
                                       or (solicitacaotipo_id =3 and sf.solicitacao_id is not null)
                                       or (solicitacaotipo_id =4 and sm.solicitacao_id is not null)
                                    ";
                    query = _repository.FromQuery<Solicitacao>(consulta);
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
                else if (request.UsuarioId > 0)
                    query = query.Where(q => q.Cliente.UsuarioClientes.Where(q1 => q1.UsuarioId == request.UsuarioId).Any() );
                   

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
                    query = query.Where(q => q.Comunicado.CentroCusto.UsuarioCentrodeCustos.Where(q1 => q1.UsuarioId == request.UsuarioId).Any());
                    if (request.FuncionarioId > 0)
                        query = query.Where(q => q.Comunicado.FuncionarioId.Equals(request.FuncionarioId)); 
                }
                else if (request.TipoSolicitacao == EnumTipoSolicitacao.Desligamento)
                {
                    query = query.IncludeDesligamento();
                    query = query.Where(q => q.Desligamento.CentroCusto.UsuarioCentrodeCustos.Where(q1 => q1.UsuarioId == request.UsuarioId).Any());
                    if (request.FuncionarioId > 0)
                        query = query.Where(q => q.Desligamento.FuncionarioId.Equals(request.FuncionarioId));

                } else if (request.TipoSolicitacao == EnumTipoSolicitacao.Ferias)
                {
                    query = query.IncludeFerias();
                    query = query.Where(q => q.Ferias.CentroCusto.UsuarioCentrodeCustos.Where(q1 => q1.UsuarioId == request.UsuarioId).Any());
                    if (request.FuncionarioId > 0)
                        query = query.Where(q => q.Ferias.FuncionarioId.Equals(request.FuncionarioId));

                }
                else if (request.TipoSolicitacao == EnumTipoSolicitacao.MudancaBase)
                {
                    query = query.IncludeMudancaBase();
                    query = query.Where(q => q.MudancaBase.CentroCusto.UsuarioCentrodeCustos.Where(q1 => q1.UsuarioId == request.UsuarioId).Any());
                    if (request.FuncionarioId > 0)
                        query = query.Where(q => q.MudancaBase.FuncionarioId.Equals(request.FuncionarioId));
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

                    if (request.FuncionarioId > 0)
                    {
                        query = query.Where(q => q.Comunicado.FuncionarioId.Equals(request.FuncionarioId) ||
                                                q.Desligamento.FuncionarioId.Equals(request.FuncionarioId) ||
                                                q.Ferias.FuncionarioId.Equals(request.FuncionarioId) ||
                                                q.MudancaBase.FuncionarioId.Equals(request.FuncionarioId));
                    }
                }

                query = query.OrderByDescending(q => q.DataSolicitacao).ThenBy(q => q.Id);

                var pagination = Pagination<SolicitacaoToPaginateResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

                return await Task.FromResult(pagination);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Parâmetros: {JsonSerializer.Serialize(request)}\n Error.Message: {ex.Message}\n Error.InnerException: {ex.InnerException?.Message}");
                return Error.Failure(ex.Message);
            }
        }
    }
}
