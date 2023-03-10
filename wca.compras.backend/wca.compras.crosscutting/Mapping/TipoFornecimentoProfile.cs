using AutoMapper;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Util;

namespace wca.compras.crosscutting.Mapping
{
    public class TipoFornecimentoProfile: Profile
    {
        public TipoFornecimentoProfile()
        {
            CreateMap<CreateTipoFornecimentoDto, TipoFornecimento>();
            CreateMap<TipoFornecimento, TipoFornecimentoDto>().ReverseMap();
            CreateMap<TipoFornecimento, ListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome))
                .ReverseMap();
        }
    }
}
