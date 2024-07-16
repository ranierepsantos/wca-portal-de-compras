using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Funcoes.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Funcoes.Queries
{
    public record FuncaoToPaginateQuery(string Termo = "") : PaginationQuery, IRequest<ErrorOr<Pagination<FuncaoResponse>>>;
    internal class FuncaoToPaginateQueryHandle : IRequestHandler<FuncaoToPaginateQuery, ErrorOr<Pagination<FuncaoResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FuncaoToPaginateQueryHandle> _logger;

        public FuncaoToPaginateQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<FuncaoToPaginateQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<FuncaoResponse>>> Handle(FuncaoToPaginateQuery request, CancellationToken cancellationToken)
        {
            string message = $"Parâmetros: {JsonSerializer.Serialize(request)}";
            _logger.LogInformation(message);

            var query = _repository.GetDbSet<Funcao>().AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.Termo))
                query = query.Where(q => q.Nome.Contains(request.Termo));

            query = query.OrderBy(o => o.Nome);

            var pagination = Pagination<FuncaoResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination);

        }
    }
}
