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
        private IClienteRepository _reposistory;
        private IMapper _mapper;
        private ILogger<ClienteByUserIdQueryHandler> _logger;

        public ClienteByUserIdQueryHandler(IClienteRepository reposistory, IMapper mapper, ILogger<ClienteByUserIdQueryHandler> logger)
        {
            _reposistory = reposistory;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ErrorOr<IList<ClienteResponse>>> Handle(ClienteByUserIdQuerie request, CancellationToken cancellationToken)
        {

            var query = _reposistory.ToQuery();
            query = query.Include("UsuarioClientes")
                         .Where(q => q.UsuarioClientes.Where(sq => sq.UsuarioId == request.UsuarioId).Count() > 0);

            var list = await query.ToListAsync();

            return _mapper.Map<List<ClienteResponse>>(list);

        }
    }
}
