using AutoMapper;
using wca.reembolso.application.Features.Usuarios.Commands;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Usuarios.Common
{
    internal class UsuarioProfile: Profile
    {
            public UsuarioProfile()
            {
                CreateMap<UsuarioCreateUpdateCommand, Usuario>();
            }
    }
}
