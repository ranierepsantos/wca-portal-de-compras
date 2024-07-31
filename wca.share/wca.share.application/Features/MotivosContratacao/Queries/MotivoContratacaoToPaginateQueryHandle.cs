using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.MotivosContratacao.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.MotivosContratacao.Queries
{
    public record MotivoContratacaoToPaginateQuery(string Termo = "") : PaginationQuery, IRequest<ErrorOr<Pagination<MotivoContratacaoResponse>>>;
    internal class MotivoContratacaoToPaginateQueryHandle : IRequestHandler<MotivoContratacaoToPaginateQuery, ErrorOr<Pagination<MotivoContratacaoResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<MotivoContratacaoToPaginateQueryHandle> _logger;

        public MotivoContratacaoToPaginateQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<MotivoContratacaoToPaginateQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<MotivoContratacaoResponse>>> Handle(MotivoContratacaoToPaginateQuery request, CancellationToken cancellationToken)
        {
            string message = $"Parâmetros: {JsonSerializer.Serialize(request)}";
            _logger.LogInformation(message);

            var query = _repository.GetDbSet<MotivoContratacao>().AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.Termo))
                query = query.Where(q => q.Nome.Contains(request.Termo));

            query = query.OrderBy(o => o.Nome);

            var pagination = Pagination<MotivoContratacaoResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination);

        }
    }
}
