using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.DocumentoComplementares.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.DocumentoComplementares.Queries
{
    public record DocumentoComplementarToPaginateQuery(string Termo = "") : PaginationQuery, IRequest<ErrorOr<Pagination<DocumentoComplementarResponse>>>;
    internal class DocumentoComplementarToPaginateQueryHandle : IRequestHandler<DocumentoComplementarToPaginateQuery, ErrorOr<Pagination<DocumentoComplementarResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DocumentoComplementarToPaginateQueryHandle> _logger;

        public DocumentoComplementarToPaginateQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<DocumentoComplementarToPaginateQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<DocumentoComplementarResponse>>> Handle(DocumentoComplementarToPaginateQuery request, CancellationToken cancellationToken)
        {
            string message = $"Parâmetros: {JsonSerializer.Serialize(request)}";
            _logger.LogInformation(message);

            var query = _repository.GetDbSet<DocumentoComplementar>().AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.Termo))
                query = query.Where(q => q.Nome.Contains(request.Termo));

            query = query.OrderBy(o => o.Nome);

            var pagination = Pagination<DocumentoComplementarResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination);

        }
    }
}
