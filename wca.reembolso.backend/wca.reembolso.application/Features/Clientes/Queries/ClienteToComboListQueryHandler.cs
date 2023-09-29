using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application6.Common;

namespace wca.reembolso.application.Features.Clientes.Queries
{
    public record ClienteToComboListQuerie(int FilialId, int usuarioId): IRequest<ErrorOr<IList<ListItem>>>;

    public sealed class ClienteToComboListQueryHandler : IRequestHandler<ClienteToComboListQuerie, ErrorOr<IList<ListItem>>>
    {
        private IRepositoryManager _repository;
        private IMapper _mapper;
        private ILogger<ClienteToComboListQueryHandler> _logger;

        public ClienteToComboListQueryHandler(IRepositoryManager repository, IMapper mapper, ILogger<ClienteToComboListQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<IList<ListItem>>> Handle(ClienteToComboListQuerie request, CancellationToken cancellationToken)
        {
            var query = _repository.ClienteRepository.ToQuery();

            if (request.FilialId > 0)
                query = query.Where(q => q.FilialId.Equals(request.FilialId));

            if (request.usuarioId > 0)
                query = query.Where(q => q.UsuarioClientes.Any(x =>  x.UsuarioId== request.usuarioId));

            var items = await query.Where(q =>  q.Ativo).OrderBy(q => q.Nome).ToListAsync(cancellationToken: cancellationToken);

            return _mapper.Map<List<ListItem>>(items);

        }
    }
}
