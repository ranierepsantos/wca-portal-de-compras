using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.TiposDespesa.Common;
using wca.reembolso.application6.Common;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.TiposDespesa.Queries
{
    public record TipoDespesaToComboListQuerie() : IRequest<ErrorOr<IList<TipoDespesaResponse>>>;

    public sealed class TipoDespesaToComboListQueryHandler : IRequestHandler<TipoDespesaToComboListQuerie, ErrorOr<IList<TipoDespesaResponse>>>
    {
        private IRepository<TipoDespesa> _reposistory;
        private IMapper _mapper;
        private ILogger<TipoDespesaToComboListQueryHandler> _logger;

        public TipoDespesaToComboListQueryHandler(IRepository<TipoDespesa> reposistory, IMapper mapper, ILogger<TipoDespesaToComboListQueryHandler> logger)
        {
            _reposistory = reposistory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<IList<TipoDespesaResponse>>> Handle(TipoDespesaToComboListQuerie request, CancellationToken cancellationToken)
        {
            var query = _reposistory.ToQuery();

            var items = await query.Where(q=> q.Ativo).OrderBy(q => q.Nome).ToListAsync();

            return _mapper.Map<List<TipoDespesaResponse>>(items);

        }
    }
}
