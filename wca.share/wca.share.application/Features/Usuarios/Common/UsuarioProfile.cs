using AutoMapper;
using wca.share.application.Features.Usuarios.Commands;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Usuarios.Common
{
    internal class UsuarioProfile: Profile
    {
            public UsuarioProfile()
            {
                CreateMap<UsuarioCreateUpdateCommand, Usuario>()
                .ForMember(dest =>  dest.UsuarioConfiguracoes, src =>  src.Ignore());
            }
    }
}
