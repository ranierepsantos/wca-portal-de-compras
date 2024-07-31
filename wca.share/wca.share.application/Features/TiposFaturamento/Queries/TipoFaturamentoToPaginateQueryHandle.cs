using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.TiposFaturamento.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.TiposFaturamento.Queries
{
    public record TipoFaturamentoToPaginateQuery(string Termo = "") : PaginationQuery, IRequest<ErrorOr<Pagination<TipoFaturamentoResponse>>>;
    internal class TipoFaturamentoToPaginateQueryHandle : IRequestHandler<TipoFaturamentoToPaginateQuery, ErrorOr<Pagination<TipoFaturamentoResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TipoFaturamentoToPaginateQueryHandle> _logger;

        public TipoFaturamentoToPaginateQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<TipoFaturamentoToPaginateQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<TipoFaturamentoResponse>>> Handle(TipoFaturamentoToPaginateQuery request, CancellationToken cancellationToken)
        {
            string message = $"Parâmetros: {JsonSerializer.Serialize(request)}";
            _logger.LogInformation(message);

            var query = _repository.GetDbSet<TipoFaturamento>().AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.Termo))
                query = query.Where(q => q.Nome.Contains(request.Termo));

            query = query.OrderBy(o => o.Nome);

            var pagination = Pagination<TipoFaturamentoResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination);

        }
    }
}
