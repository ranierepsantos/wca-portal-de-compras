using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Escalas.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Escalas.Queries
{
    public record EscalaToPaginateQuery(string Termo = "") : PaginationQuery, IRequest<ErrorOr<Pagination<EscalaResponse>>>;
    internal class EscalaToPaginateQueryHandle : IRequestHandler<EscalaToPaginateQuery, ErrorOr<Pagination<EscalaResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<EscalaToPaginateQueryHandle> _logger;

        public EscalaToPaginateQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<EscalaToPaginateQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<EscalaResponse>>> Handle(EscalaToPaginateQuery request, CancellationToken cancellationToken)
        {
            string message = $"Parâmetros: {JsonSerializer.Serialize(request)}";
            _logger.LogInformation(message);

            var query = _repository.GetDbSet<Escala>().AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.Termo))
                query = query.Where(q => q.Nome.Contains(request.Termo));

            query = query.OrderBy(o => o.Nome);

            var pagination = Pagination<EscalaResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination);

        }
    }
}
