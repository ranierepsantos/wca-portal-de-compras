using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.TiposDespesa.Common;
using wca.reembolso.domain.Common.Enum;

namespace wca.reembolso.application.Features.TiposDespesa.Queries
{
    public record TipoDespesaToComboListQuerie(int ListarTipoDespesa  = (int) EnumListarTipoDespesa.MostrarTodas) : IRequest<ErrorOr<IList<TipoDespesaResponse>>>;

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

            if (request.ListarTipoDespesa == (int) EnumListarTipoDespesa.ExibirColaborador)
                query = query.Where(q => q.ExibirParaColaborador);
            else if (request.ListarTipoDespesa == (int)EnumListarTipoDespesa.NaoExibirColaborador)
                query = query.Where(q => !q.ExibirParaColaborador);

            var items = await query.Where(q=> q.Ativo).OrderBy(q => q.Nome).ToListAsync(cancellationToken: cancellationToken);

            return _mapper.Map<List<TipoDespesaResponse>>(items);

        }
    }
}
