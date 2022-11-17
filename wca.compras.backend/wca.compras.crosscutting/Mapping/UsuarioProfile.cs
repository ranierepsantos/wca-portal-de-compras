using AutoMapper;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;

namespace wca.compras.crosscutting.Mapping
{
    public class UsuarioProfile: Profile
    {
        public UsuarioProfile()
        {
            CreateMap<UsuarioDto, Usuario>().ReverseMap();
            CreateMap<CreateUsuarioDto, Usuario>();
            CreateMap<UpdateUsuarioDto, Usuario>();
        }
    }
}
