using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Gestors.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Gestors.Queries
{
    public record GestorToPaginateQuery(string Termo = "") : PaginationQuery, IRequest<ErrorOr<Pagination<GestorResponse>>>;
    internal class GestorToPaginateQueryHandle : IRequestHandler<GestorToPaginateQuery, ErrorOr<Pagination<GestorResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GestorToPaginateQueryHandle> _logger;

        public GestorToPaginateQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<GestorToPaginateQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<GestorResponse>>> Handle(GestorToPaginateQuery request, CancellationToken cancellationToken)
        {
            string message = $"Parâmetros: {JsonSerializer.Serialize(request)}";
            _logger.LogInformation(message);

            var query = _repository.GetDbSet<Gestor>().AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.Termo))
                query = query.Where(q => q.Nome.Contains(request.Termo));

            query = query.OrderBy(o => o.Nome);

            var pagination = Pagination<GestorResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination);

        }
    }
}
