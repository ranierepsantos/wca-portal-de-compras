using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Clientes.Queries
{
    public record ClienteUsersByClienteQuerie(int ClienteId) : IRequest<ErrorOr<IList<int>>>;
    public class ClienteUsersByClienteQueryHandler : IRequestHandler<ClienteUsersByClienteQuerie, ErrorOr<IList<int>>>
    {
        private readonly IRepository<UsuarioClientes> _reposistory;
        private readonly IMapper _mapper;
        private readonly ILogger<ClienteUsersByClienteQueryHandler> _logger;

        public ClienteUsersByClienteQueryHandler(IRepository<UsuarioClientes> reposistory, IMapper mapper, ILogger<ClienteUsersByClienteQueryHandler> logger)
        {
            _reposistory = reposistory;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ErrorOr<IList<int>>> Handle(ClienteUsersByClienteQuerie request, CancellationToken cancellationToken)
        {

            _logger.LogInformation("Listando usuários por cliente");


            var query = _reposistory.ToQuery().Where(q => q.ClienteId.Equals(request.ClienteId));
            
            var list = await query.Select(q => new
            {
                id = q.UsuarioId
            }).ToListAsync();

            return list.Select(q => q.id).ToList();

        }
    }
}
