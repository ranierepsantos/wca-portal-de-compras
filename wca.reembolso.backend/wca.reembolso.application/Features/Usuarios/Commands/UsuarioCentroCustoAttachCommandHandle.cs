using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Clientes.Commands;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Usuarios.Commands
{
    public record UsuarioCentroCustoAttachCommand (
        int UsuarioId,
        int [] CentroCustoIds
    ): IRequest<ErrorOr<bool>>;

    internal class UsuarioCentroCustoAttachCommandHandle : IRequestHandler<UsuarioCentroCustoAttachCommand, ErrorOr<bool>>
    {

        private IRepositoryManager _repository;
        private ILogger<ClienteUsuarioAttachCommandHandler> _logger;

        public UsuarioCentroCustoAttachCommandHandle(IRepositoryManager repository, ILogger<ClienteUsuarioAttachCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ErrorOr<bool>> Handle(UsuarioCentroCustoAttachCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Excluíndo relação de usuario x centro de custo");

            await _repository.ExecuteCommandAsync($"DELETE FROM UsuarioCentrodeCustos WHERE usuarioid = {request.UsuarioId}");

            _logger.LogInformation("vinculando usuario x centro de custo");

            for(int idx = 0; idx < request.CentroCustoIds.Length; idx++)
            {
                UsuarioCentrodeCustos usuarioCentrodeCustos = new()
                {
                    UsuarioId = request.UsuarioId,
                    CentroCustoId = request.CentroCustoIds[idx],
                };
                _repository.GetDbSet<UsuarioCentrodeCustos>().Add(usuarioCentrodeCustos);
            }
            await _repository.SaveAsync();

            return true;
        }
    }

}
