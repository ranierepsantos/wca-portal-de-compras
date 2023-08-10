using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Clientes.Common;

namespace wca.reembolso.application.Features.Clientes.Queries
{
    public record ClienteByIdQuerie(int id): IRequest<ErrorOr<ClienteResponse>>;
    public class ClienteByIdQueryHandler : IRequestHandler<ClienteByIdQuerie, ErrorOr<ClienteResponse>>
    {
        private IClienteRepository _reposistory;
        private IMapper _mapper;
        private ILogger<ClienteByIdQueryHandler> _logger;

        public ClienteByIdQueryHandler(IClienteRepository reposistory, IMapper mapper, ILogger<ClienteByIdQueryHandler> logger)
        {
            _reposistory = reposistory;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ErrorOr<ClienteResponse>> Handle(ClienteByIdQuerie request, CancellationToken cancellationToken)
        {

            var cliente = await _reposistory.GetByIdAsync(request.id);

            if (cliente == null)
            {
                _logger.LogError($"Cliente não localizado!");
                return Error.NotFound(description: $"Cliente não localizado!");
            }

            return _mapper.Map<ClienteResponse>(cliente);
            
        }
    }
}
