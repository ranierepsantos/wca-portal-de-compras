using AutoMapper;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Util;

namespace wca.compras.crosscutting.Mapping
{
    public class FilialProfile: Profile
    {
        public FilialProfile()
        {
            CreateMap<Filial, FilialDto>().ReverseMap();
            CreateMap<CreateFilialDto, Filial>();
            CreateMap<Filial, ListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome));
        }
    }
}
