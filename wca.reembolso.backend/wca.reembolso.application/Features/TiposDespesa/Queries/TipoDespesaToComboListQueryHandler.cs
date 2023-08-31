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
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TipoDespesaToComboListQueryHandler> _logger;

        public TipoDespesaToComboListQueryHandler(IRepositoryManager repository, IMapper mapper, ILogger<TipoDespesaToComboListQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<IList<TipoDespesaResponse>>> Handle(TipoDespesaToComboListQuerie request, CancellationToken cancellationToken)
        {

            _logger.LogInformation("Buscando tipo de despesa para lista/combo");

            var query = _repository.TipoDespesaRepository.ToQuery();

            var items = await query.Where(q=> q.Ativo).OrderBy(q => q.Nome).ToListAsync(cancellationToken: cancellationToken);

            return _mapper.Map<List<TipoDespesaResponse>>(items);

        }
    }
}
