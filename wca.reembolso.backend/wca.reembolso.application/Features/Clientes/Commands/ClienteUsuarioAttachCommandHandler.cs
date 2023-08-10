using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Clientes.Commands
{
    public record ClienteUsuarioAttachCommand (
        int UsuarioId,
        int[] ClienteIds 
    ) : IRequest<ErrorOr<bool>>;

    public class ClienteUsuarioAttachCommandHandler : IRequestHandler<ClienteUsuarioAttachCommand, ErrorOr<bool>>
    {
        private IRepository<UsuarioClientes> _reposistory;
        private ILogger<ClienteUsuarioAttachCommandHandler> _logger;

        public ClienteUsuarioAttachCommandHandler(IRepository<UsuarioClientes> reposistory, ILogger<ClienteUsuarioAttachCommandHandler> logger)
        {
            _reposistory = reposistory;
            _logger = logger;
        }

        async Task<ErrorOr<bool>> IRequestHandler<ClienteUsuarioAttachCommand, ErrorOr<bool>>.Handle(ClienteUsuarioAttachCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ClienteUsuarioAttachCommandHandler - início");

            await _reposistory.ExecuteCommandAsync($"DELETE FROM UsuarioClientes WHERE usuario_id = {request.UsuarioId}");

            _logger.LogInformation("ClienteUsuarioAttachCommandHandler - vinculando cliente x usuario");

            for (int idx = 0; idx < request.ClienteIds.Length; idx++)
            {
                UsuarioClientes usuarioCliente = new UsuarioClientes() { 
                    UsuarioId = request.UsuarioId,
                    ClienteId = request.ClienteIds[idx],
                };
                _reposistory.Create(usuarioCliente);
            }
            await _reposistory.SaveChangesAsync();

            return true;
        }
    }
}
