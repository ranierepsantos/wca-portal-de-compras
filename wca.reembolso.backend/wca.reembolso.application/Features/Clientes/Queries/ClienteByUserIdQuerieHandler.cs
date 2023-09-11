using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Clientes.Common;

namespace wca.reembolso.application.Features.Clientes.Queries
{
    public record ClienteByUserIdQuerie(int UsuarioId) : IRequest<ErrorOr<IList<ClienteResponse>>>;
    public class ClienteByUserIdQueryHandler : IRequestHandler<ClienteByUserIdQuerie, ErrorOr<IList<ClienteResponse>>>
    {
        private IRepositoryManager _repository;
        private IMapper _mapper;
        private ILogger<ClienteByUserIdQueryHandler> _logger;

        public ClienteByUserIdQueryHandler(IRepositoryManager repository, IMapper mapper, ILogger<ClienteByUserIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ErrorOr<IList<ClienteResponse>>> Handle(ClienteByUserIdQuerie request, CancellationToken cancellationToken)
        {

            var query = _repository.ClienteRepository.ToQuery();
            query = query.Where(q => q.UsuarioClientes.Any(sq => sq.UsuarioId == request.UsuarioId));

            var list = await query.ToListAsync(cancellationToken: cancellationToken);

            return _mapper.Map<List<ClienteResponse>>(list);

        }
    }
}
