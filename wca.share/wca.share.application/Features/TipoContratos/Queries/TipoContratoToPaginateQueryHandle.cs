using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.TipoContratos.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.TipoContratos.Queries
{
    public record TipoContratoToPaginateQuery(string Termo = "") : PaginationQuery, IRequest<ErrorOr<Pagination<TipoContratoResponse>>>;
    internal class TipoContratoToPaginateQueryHandle : IRequestHandler<TipoContratoToPaginateQuery, ErrorOr<Pagination<TipoContratoResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TipoContratoToPaginateQueryHandle> _logger;

        public TipoContratoToPaginateQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<TipoContratoToPaginateQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<TipoContratoResponse>>> Handle(TipoContratoToPaginateQuery request, CancellationToken cancellationToken)
        {
            string message = $"Parâmetros: {JsonSerializer.Serialize(request)}";
            _logger.LogInformation(message);

            var query = _repository.GetDbSet<TipoContrato>().AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.Termo))
                query = query.Where(q => q.Nome.Contains(request.Termo));

            query = query.OrderBy(o => o.Nome);

            var pagination = Pagination<TipoContratoResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination);

        }
    }
}
