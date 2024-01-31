using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Clientes.Common;

namespace wca.share.application.Features.Clientes.Queries
{
    public record ClienteByIdQuerie(int id): IRequest<ErrorOr<ClienteResponse>>;
    public class ClienteByIdQueryHandler : IRequestHandler<ClienteByIdQuerie, ErrorOr<ClienteResponse>>
    {
        private IRepositoryManager _repository;
        private IMapper _mapper;
        private ILogger<ClienteByIdQueryHandler> _logger;

        public ClienteByIdQueryHandler(IRepositoryManager repository, IMapper mapper, ILogger<ClienteByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ErrorOr<ClienteResponse>> Handle(ClienteByIdQuerie request, CancellationToken cancellationToken)
        {

            var cliente = await _repository.ClienteRepository.ToQuery().Where(q =>  q.Id.Equals( request.id)).FirstOrDefaultAsync(cancellationToken);

            if (cliente == null)
            {
                _logger.LogError($"Cliente não localizado!");
                return Error.NotFound(description: $"Cliente não localizado!");
            }

            return _mapper.Map<ClienteResponse>(cliente);
            
        }
    }
}
