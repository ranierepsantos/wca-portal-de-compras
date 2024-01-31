using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Clientes.Commands
{
    public record ClienteUsuarioAttachCommand (
        int UsuarioId,
        int[] ClienteIds 
    ) : IRequest<ErrorOr<bool>>;

    public class ClienteUsuarioAttachCommandHandler : IRequestHandler<ClienteUsuarioAttachCommand, ErrorOr<bool>>
    {
        private IRepositoryManager _repository;
        private ILogger<ClienteUsuarioAttachCommandHandler> _logger;

        public ClienteUsuarioAttachCommandHandler(IRepositoryManager repository, ILogger<ClienteUsuarioAttachCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        async Task<ErrorOr<bool>> IRequestHandler<ClienteUsuarioAttachCommand, ErrorOr<bool>>.Handle(ClienteUsuarioAttachCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ClienteUsuarioAttachCommandHandler - início");

            await _repository.ExecuteCommandAsync($"DELETE FROM UsuarioClientes WHERE usuario_id = {request.UsuarioId}");

            _logger.LogInformation("ClienteUsuarioAttachCommandHandler - vinculando cliente x usuario");

            for (int idx = 0; idx < request.ClienteIds.Length; idx++)
            {
                UsuarioClientes usuarioCliente = new UsuarioClientes() { 
                    UsuarioId = request.UsuarioId,
                    ClienteId = request.ClienteIds[idx],
                };
                _repository.UsuarioClientesRepository.Create(usuarioCliente);
            }
            await _repository.SaveAsync();

            return true;
        }
    }
}
