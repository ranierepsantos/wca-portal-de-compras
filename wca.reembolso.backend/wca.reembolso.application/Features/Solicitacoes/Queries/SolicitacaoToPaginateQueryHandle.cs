using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Common;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Solicitacoes.Common;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Solicitacaos.Queries
{

    public record SolicitacaoPaginateQuery(DateTime? DataIni, DateTime? DataFim, int ColaboradorId =0 , int GestorId = 0, int ClienteId = 0) : PaginationQuery, IRequest<ErrorOr<Pagination<SolicitacaoResponse>>>;
    public sealed class SolicitacaoToPaginateQueryHandle : 
        IRequestHandler<SolicitacaoPaginateQuery, ErrorOr<Pagination<SolicitacaoResponse>>>
    {
        private readonly IRepository<Solicitacao> _reposistory;
        private readonly IMapper _mapper;
        private readonly ILogger<SolicitacaoToPaginateQueryHandle> _logger;
        public SolicitacaoToPaginateQueryHandle(IRepository<Solicitacao> reposistory, IMapper mapper, ILogger<SolicitacaoToPaginateQueryHandle> logger)
        {
            _reposistory = reposistory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<SolicitacaoResponse>>> Handle(
            SolicitacaoPaginateQuery request, CancellationToken cancellationToken)
        {

            if (request.DataIni > request.DataFim || (request.DataIni != null && request.DataFim is null) || (request.DataFim != null && request.DataIni is null))
            {
                return Error.Validation("Data início ou fim inválida!");
            }

            var query = _reposistory.ToQuery();

            if (request.FilialId > 1)
                query = query.Include(i => i.Cliente)
                        .Where(q => q.Cliente.FilialId.Equals(request.FilialId));

            if (request.ColaboradorId > 0)
                query = query.Where(q => q.ColaboradorId.Equals(request.ColaboradorId));

            if (request.GestorId > 0)
                query = query.Where(q => q.GestorId.Equals(request.GestorId));

            if (request.ClienteId > 0)
                query = query.Where(q => q.ClienteId.Equals(request.ClienteId));

            if (request.DataIni != null && request.DataFim != null)
                query = query.Where(c => c.DataSolicitacao >= request.DataIni && c.DataSolicitacao <= request.DataFim);

            query = query.OrderBy(q => q.Id);

            var pagination = Pagination<SolicitacaoResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination) ;
        }
    }
}
