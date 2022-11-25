using AutoMapper;
using wca.compras.domain.Entities;
using wca.compras.domain.Util;
using wca.compras.domain.Dtos;

namespace wca.compras.crosscutting.Mapping
{
    public class PerfilProfile: Profile
    {
        public PerfilProfile()
        {
            CreateMap<PerfilDto, Perfil>().ReverseMap();
            CreateMap<CreatePerfilDto, Perfil>();
            CreateMap<UpdatePerfilDto, Perfil>();
            CreateMap<Perfil, PerfilPermissoesDto>();
            CreateMap<Perfil, ListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome));
        }
    }
}
