using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Common;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.TiposDespesa.Common;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.TiposDespesa.Queries
{

    public record TipoDespesaPaginateQuery(string Termo = "") : PaginationQuery, IRequest<ErrorOr<Pagination<TipoDespesaResponse>>>;
    public sealed class TipoDespesaToPaginateQueryHandle :
        IRequestHandler<TipoDespesaPaginateQuery, ErrorOr<Pagination<TipoDespesaResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TipoDespesaToPaginateQueryHandle> _logger;
        public TipoDespesaToPaginateQueryHandle(
            IRepositoryManager repository, IMapper mapper, ILogger<TipoDespesaToPaginateQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<TipoDespesaResponse>>> Handle(
            TipoDespesaPaginateQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.TipoDespesaRepository.ToQuery();

            if (!string.IsNullOrEmpty(request.Termo))
                query = query.Where(q => q.Nome.Contains(request.Termo));

            var pagination = Pagination<TipoDespesaResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination);
        }
    }
}
