using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.domain.Entities;


namespace wca.reembolso.application.Features.TiposDespesa.Commands
{
    public record TipoDespesaPerfilAttachCommand (
        int PerfilId,
        int [] TipoDespesaIds
    ): IRequest<ErrorOr<bool>>;

    public class TipoDespesaPerfilAttachCommandHandle : IRequestHandler<TipoDespesaPerfilAttachCommand, ErrorOr<bool>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<TipoDespesaPerfilAttachCommandHandle> _logger;

        public TipoDespesaPerfilAttachCommandHandle(IRepositoryManager repository, ILogger<TipoDespesaPerfilAttachCommandHandle> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ErrorOr<bool>> Handle(TipoDespesaPerfilAttachCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Excluíndo relação de perfil x tipo de despesa");

            await _repository.ExecuteCommandAsync($"DELETE FROM Perfil_TipoDespesa WHERE perfil_id = {request.PerfilId}");

            _logger.LogInformation("vinculando perfil x tipo de despesa");

            for (int idx = 0; idx < request.TipoDespesaIds.Length; idx++)
            {
                PerfilTipoDespesa perfilTipoDespesa = new()
                {
                    PerfilId = request.PerfilId,
                    TipoDespesaId = request.TipoDespesaIds[idx],
                };
                _repository.GetDbSet<PerfilTipoDespesa>().Add(perfilTipoDespesa);
            }
            await _repository.SaveAsync();

            return true;
        }
    }
}
