using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application6.Common;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Usuarios.Commands
{
    public record UsuarioCreateUpdateCommand (
      int Id,
      string Nome,
      string Email,
      bool Ativo,
      string? Celular,
      UsuarioConfiguracoes? Configuracoes
    ) : IRequest<ErrorOr<bool>>;

    public sealed class UsuarioCreateUpdateCommandHandle : IRequestHandler<UsuarioCreateUpdateCommand, ErrorOr<bool>>
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
            _logger.LogInformation("Iniciando criação de usuario");


            var usuario = await _repository.UsuarioRepository.ToQuery()
                           .FirstOrDefaultAsync(q => q.Id.Equals(request.Id));

            if (usuario == null)
            {
                usuario = _mapper.Map<Usuario>(request);
                _repository.UsuarioRepository.Create(usuario);
            }else
            {
                usuario.Nome = request.Nome;
                usuario.Email = request.Email;
                usuario.Ativo = request.Ativo;
                usuario.Celular = request.Celular;

                _repository.GetDbSet<Usuario>().Entry(usuario).State = EntityState.Modified;
            }

            UsuarioConfiguracoes _userConfig = new()
            {
                UsuarioId = request.Id,
                NotificarPorChatBot = request.Configuracoes?.NotificarPorChatBot ?? false,
                NotificarPorEmail = request.Configuracoes?.NotificarPorEmail ?? false
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
