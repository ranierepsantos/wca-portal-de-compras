using AutoMapper;
using wca.compras.domain.Entities;
using wca.compras.domain.Util;
using wca.compras.domain.Dtos;

namespace wca.compras.crosscutting.Mapping
{
    public class PermissaoProfile: Profile
    {
        public PermissaoProfile()
        {
            CreateMap<PermissaoDto, Permissao>().ReverseMap();
            CreateMap<CreatePermissaoDto, Permissao>();
            CreateMap<UpdatePermissaoDto, Permissao>();
            CreateMap<Permissao, PermissoesDto>();
            CreateMap<Permissao, ListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome));
        }
    }
}
