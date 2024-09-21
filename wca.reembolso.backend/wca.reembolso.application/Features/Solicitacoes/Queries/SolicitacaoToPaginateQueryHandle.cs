using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Common;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Solicitacoes.Common;

namespace wca.reembolso.application.Features.Solicitacaos.Queries
{

    public record SolicitacaoPaginateQuery(int UsuarioId = 0, int ClienteId = 0, DateTime? DataIni = null, DateTime? DataFim = null, int[]? Status = null, int[]? CentroCustoIds = null) : PaginationQuery, IRequest<ErrorOr<Pagination<SolicitacaoToPaginateResponse>>>;
    public sealed class SolicitacaoToPaginateQueryHandle : 
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

            if (request.DataIni > request.DataFim || (request.DataIni != null && request.DataFim is null) || (request.DataFim != null && request.DataIni is null))
            {
                return Error.Validation("Data início ou fim inválida!");
            }

            var query = _repository.SolicitacaoRepository.ToQuery();
            query = query.Include(i => i.Cliente)
                         .Include(q => q.Colaborador)
                         .Include(q => q.CentroCusto)
                         .Include(q => q.Despesa)
                            .ThenInclude(q => q.TipoDespesa)
                         .Include(q => q.SolicitacaoHistorico.OrderByDescending(f => f.DataHora));

            if (request.FilialId > 1)
             query = query.Where(q => q.Cliente.FilialId.Equals(request.FilialId));

            if (request.UsuarioId > 0)
                query = query.Where(q => q.ColaboradorId.Equals(request.UsuarioId));

            if (request.ClienteId > 0)
                query = query.Where(q => q.ClienteId.Equals(request.ClienteId));

            if (request.Status?.Length > 0)
                query = query.Where(q => request.Status.Contains(q.Status));

            if (request.CentroCustoIds?.Length > 0)
                query = query.Where(c => request.CentroCustoIds.Contains(c.CentroCustoId));

            if (request.DataIni != null && request.DataFim != null)
            {
                var dataFim = request.DataFim.Value.AddHours(23).AddMinutes(59);
                query = query.Where(c => c.DataSolicitacao >= request.DataIni && c.DataSolicitacao <= dataFim);
            }

            query = query.OrderByDescending(q => q.Id);

            var pagination = Pagination<SolicitacaoToPaginateResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination) ;
        }
    }
}
