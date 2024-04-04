using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Usuarios.Commands
{
    public record UsuarioCreateUpdateCommand (
      int Id,
      string Nome,
      string Email,
      bool Ativo,
      string? Celular,
      UsuarioConfiguracoes? UsuarioConfiguracoes
    ) : IRequest<ErrorOr<bool>>;

    internal sealed class UsuarioCreateUpdateCommandHandle : IRequestHandler<UsuarioCreateUpdateCommand, ErrorOr<bool>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UsuarioCreateUpdateCommandHandle> _logger;

        public UsuarioCreateUpdateCommandHandle(IRepositoryManager repository, IMapper mapper, ILogger<UsuarioCreateUpdateCommandHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<bool>> Handle(UsuarioCreateUpdateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando criação / atualização de usuario");


            var usuario = await _repository.UsuarioRepository.ToQuery()
                           .FirstOrDefaultAsync(q => q.Id.Equals(request.Id), cancellationToken: cancellationToken);

            if (usuario == null)
            {
                usuario = _mapper.Map<Usuario>(request);
                _repository.UsuarioRepository.Create(entity: usuario);
                _logger.LogInformation($"Criando usuario {usuario.Nome}");
            }
            else
            {
                _mapper.Map(request, usuario);
                _repository.GetDbSet<Usuario>().Entry(usuario).State = EntityState.Modified;
                _logger.LogInformation($"Atualizando usuario {usuario.Nome}");
            }

            UsuarioConfiguracoes _userConfig = new()
            {
                UsuarioId = request.Id,
                NotificarPorChatBot = request.UsuarioConfiguracoes?.NotificarPorChatBot ?? false,
                NotificarPorEmail = request.UsuarioConfiguracoes?.NotificarPorEmail ?? false
            };

            UsuarioConfiguracoes? configuracoes = await _repository.GetDbSet<UsuarioConfiguracoes>()
                                                 .Where(q => q.UsuarioId.Equals(request.Id))
                                                 .AsNoTracking()
                                                 .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (configuracoes == null)
                _repository.GetDbSet<UsuarioConfiguracoes>().Add(_userConfig);
            else
                _repository.GetDbSet<UsuarioConfiguracoes>().Entry(_userConfig).State = EntityState.Modified;

            await _repository.SaveAsync();
            return true;
        }
    }
}
