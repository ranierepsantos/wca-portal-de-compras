using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Clientes.Common;

namespace wca.share.application.Features.Clientes.Queries
{
    public record ClienteByUserIdQuerie(int UsuarioId) : IRequest<ErrorOr<IList<ClienteResponse>>>;
    internal sealed class ClienteByUserIdQueryHandle : IRequestHandler<ClienteByUserIdQuerie, ErrorOr<IList<ClienteResponse>>>
    {
        private IRepositoryManager _repository;
        private IMapper _mapper;
        private ILogger<ClienteByUserIdQueryHandle> _logger;

        public ClienteByUserIdQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<ClienteByUserIdQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ErrorOr<IList<ClienteResponse>>> Handle(ClienteByUserIdQuerie request, CancellationToken cancellationToken)
        {

            var query = _repository.ClienteRepository.ToQuery()
                .Where(q => q.UsuarioClientes.Any(sq => sq.UsuarioId == request.UsuarioId));

            query = query.OrderBy(x => x.Nome);

            var list = await query.ToListAsync(cancellationToken: cancellationToken);

            return _mapper.Map<List<ClienteResponse>>(list);

        }
    }
}
