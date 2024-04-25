using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Assuntos.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Assuntos.Queries
{
    public record AssuntoToPaginateQuery(string Termo = "") : PaginationQuery, IRequest<ErrorOr<Pagination<AssuntoResponse>>>;
    internal class AssuntoToPaginateQueryHandle : IRequestHandler<AssuntoToPaginateQuery, ErrorOr<Pagination<AssuntoResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<AssuntoToPaginateQueryHandle> _logger;

        public AssuntoToPaginateQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<AssuntoToPaginateQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<AssuntoResponse>>> Handle(AssuntoToPaginateQuery request, CancellationToken cancellationToken)
        {
            string message = $"Parâmetros: {JsonSerializer.Serialize(request)}";
            _logger.LogInformation(message);

            var query = _repository.GetDbSet<Assunto>().AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.Termo))
                query = query.Where(q => q.Nome.Contains(request.Termo));
            
            query = query.OrderBy(o => o.Nome);

            var pagination = Pagination<AssuntoResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination);

        }
    }
}
