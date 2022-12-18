using AutoMapper;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Util;

namespace wca.compras.crosscutting.Mapping
{
    public class UsuarioProfile: Profile
    {
        public UsuarioProfile()
        {
            CreateMap<UsuarioDto, Usuario>().ReverseMap();
            CreateMap<CreateUsuarioDto, Usuario>();
            CreateMap<UpdateUsuarioDto, Usuario>();
            CreateMap<Usuario, ListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome));
        }
    }
}
