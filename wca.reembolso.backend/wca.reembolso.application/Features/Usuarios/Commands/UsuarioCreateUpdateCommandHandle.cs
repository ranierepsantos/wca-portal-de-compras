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
      IList<ListItem>? Filial
    ): IRequest<ErrorOr<bool>>;

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
                _repository.GetDbSet<Usuario>().Entry(usuario).State = EntityState.Modified;
            }

            //if (request.Filial != null)
            //{
            //    List<FilialUsuario> toRemove = usuario.FilialUsuario
            //    .Where(x => !request.Filial.Where(q => q.Value == x.FilialId).Any())
            //    .Where(x => x.FilialId != 0)
            //    .ToList();

            //    foreach (var item in toRemove)
            //    {
            //        _repository.FilialUsuarioRepository.Delete(item);
            //    }

            //    List<ListItem> toAdd = request.Filial
            //        .Where(x => !usuario.FilialUsuario.Any(q => q.FilialId.Equals(x.Value)))
            //    .ToList();

            //    foreach (var item in toAdd)
            //    {
            //        var _filial = new FilialUsuario() { 
            //            FilialId = item.Value,
            //            UsuarioId = request.Id
            //        };
                    
            //        usuario.FilialUsuario.Add(_filial);
            //    }
            //}
            await _repository.SaveAsync();
            return true;
        }
    }
}
