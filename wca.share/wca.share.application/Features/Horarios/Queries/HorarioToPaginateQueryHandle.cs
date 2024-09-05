using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Horarios.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Horarios.Queries
{
    public record HorarioToPaginateQuery(string Termo = "") : PaginationQuery, IRequest<ErrorOr<Pagination<HorarioResponse>>>;
    internal class HorarioToPaginateQueryHandle : IRequestHandler<HorarioToPaginateQuery, ErrorOr<Pagination<HorarioResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<HorarioToPaginateQueryHandle> _logger;

        public HorarioToPaginateQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<HorarioToPaginateQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<HorarioResponse>>> Handle(HorarioToPaginateQuery request, CancellationToken cancellationToken)
        {
            string message = $"Parâmetros: {JsonSerializer.Serialize(request)}";
            _logger.LogInformation(message);

            var query = _repository.GetDbSet<Horario>().AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.Termo))
                query = query.Where(q => q.Nome.Contains(request.Termo));

            query = query.OrderBy(o => o.Nome);

            var pagination = Pagination<HorarioResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination);

        }
    }
}
