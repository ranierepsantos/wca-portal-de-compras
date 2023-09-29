using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Clientes.Queries;
using wca.reembolso.application6.Common;

namespace wca.reembolso.application.Features.Usuarios.Queries
{
    public record UsuarioGetFiliais(int usuarioId): IRequest<ErrorOr<IList<ListItem>>>;
    public sealed class UsuarioGetFiliaisQueryHandle : IRequestHandler<UsuarioGetFiliais, ErrorOr<IList<ListItem>>>
    {
        private IRepositoryManager _repository;
        private IMapper _mapper;
        private ILogger<UsuarioGetFiliaisQueryHandle> _logger;

        public UsuarioGetFiliaisQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<UsuarioGetFiliaisQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<IList<ListItem>>> Handle(UsuarioGetFiliais request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando retorno de filiais por Usuário");

            var list = await _repository.FilialRepository.ToQuery()
                .Include("FilialUsuario")
                .Where(q =>  q.FilialUsuario.Any(x =>  x.UsuarioId == request.usuarioId))
                .ToListAsync(cancellationToken: cancellationToken);

            return _mapper.Map<List<ListItem>>(list);


        }
    }
}
