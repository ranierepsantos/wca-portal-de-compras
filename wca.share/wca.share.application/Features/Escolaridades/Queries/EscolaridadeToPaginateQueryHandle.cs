using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Escolaridades.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Escolaridades.Queries
{
    public record EscolaridadeToPaginateQuery(string Termo = "") : PaginationQuery, IRequest<ErrorOr<Pagination<EscolaridadeResponse>>>;
    internal class EscolaridadeToPaginateQueryHandle : IRequestHandler<EscolaridadeToPaginateQuery, ErrorOr<Pagination<EscolaridadeResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<EscolaridadeToPaginateQueryHandle> _logger;

        public EscolaridadeToPaginateQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<EscolaridadeToPaginateQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<EscolaridadeResponse>>> Handle(EscolaridadeToPaginateQuery request, CancellationToken cancellationToken)
        {
            string message = $"Parâmetros: {JsonSerializer.Serialize(request)}";
            _logger.LogInformation(message);

            var query = _repository.GetDbSet<Escolaridade>().AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.Termo))
                query = query.Where(q => q.Nome.Contains(request.Termo));

            query = query.OrderBy(o => o.Nome);

            var pagination = Pagination<EscolaridadeResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination);

        }
    }
}
