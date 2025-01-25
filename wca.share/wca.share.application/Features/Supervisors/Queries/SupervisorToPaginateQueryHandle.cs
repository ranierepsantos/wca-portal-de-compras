using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Supervisors.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Supervisors.Queries
{
    public record SupervisorToPaginateQuery(string Termo = "") : PaginationQuery, IRequest<ErrorOr<Pagination<SupervisorResponse>>>;
    internal class SupervisorToPaginateQueryHandle : IRequestHandler<SupervisorToPaginateQuery, ErrorOr<Pagination<SupervisorResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SupervisorToPaginateQueryHandle> _logger;

        public SupervisorToPaginateQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<SupervisorToPaginateQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<SupervisorResponse>>> Handle(SupervisorToPaginateQuery request, CancellationToken cancellationToken)
        {
            string message = $"Parâmetros: {JsonSerializer.Serialize(request)}";
            _logger.LogInformation(message);

            var query = _repository.GetDbSet<Supervisor>().AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.Termo))
                query = query.Where(q => q.Nome.Contains(request.Termo));

            query = query.OrderBy(o => o.Nome);

            var pagination = Pagination<SupervisorResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination);

        }
    }
}
