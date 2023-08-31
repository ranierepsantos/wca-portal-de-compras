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
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ClienteUsersByClienteQueryHandler> _logger;

        public ClienteUsersByClienteQueryHandler(IRepositoryManager repository, IMapper mapper, ILogger<ClienteUsersByClienteQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ErrorOr<IList<int>>> Handle(ClienteUsersByClienteQuerie request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Listando usuários por cliente");

            var query = _repository.UsuarioClientesRepository.ToQuery()
                        .Where(q => q.ClienteId.Equals(request.ClienteId));
            
            var list = await query.Select(q => new
            {
                id = q.UsuarioId
            }).ToListAsync(cancellationToken: cancellationToken);

            return list.Select(q => q.id).ToList();

        }
    }
}
