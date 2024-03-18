using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Common;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Faturamentos.Common;

namespace wca.reembolso.application.Features.Faturamentos.Queries
{

    public record FaturamentoPaginateQuery(DateTime? DataIni, DateTime? DataFim, int ClienteId = 0, int Status = 0) : PaginationQuery, IRequest<ErrorOr<Pagination<FaturamentoPaginateResponse>>>;
    public sealed class FaturamentoToPaginateQueryHandle : IRequestHandler<FaturamentoPaginateQuery, ErrorOr<Pagination<FaturamentoPaginateResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FaturamentoToPaginateQueryHandle> _logger;

        public FaturamentoToPaginateQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<FaturamentoToPaginateQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<FaturamentoPaginateResponse>>> Handle(FaturamentoPaginateQuery request, CancellationToken cancellationToken)
        {
            if (request.DataIni > request.DataFim || (request.DataIni != null && request.DataFim is null) || (request.DataFim != null && request.DataIni is null))
            {
                return Error.Validation("Data início ou fim inválida!");
            }

            var query = _repository.FaturamentoRepository.ToQuery();
            query = query.Include(i => i.Cliente)
                         .Include(i => i.FaturamentoHistorico.OrderByDescending(f => f.DataHora));
            

            if (request.FilialId > 0)
                query = query.Where(q => q.Cliente.FilialId.Equals(request.FilialId));

            if (request.ClienteId > 0)
                query = query.Where(q => q.ClienteId.Equals(request.ClienteId));

            if (request.Status > 0)
                query = query.Where(q => q.Status.Equals(request.Status));

            if (request.DataIni != null && request.DataFim != null)
            {
                var dataFim = request.DataFim.Value.AddHours(23).AddMinutes(59);
                query = query.Where(c => c.DataCriacao >= request.DataIni && c.DataCriacao <= dataFim);
            }
                

            query = query.OrderBy(q => q.Id);


            var pagination = Pagination<FaturamentoPaginateResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination);
        }
    }
}
    
    
