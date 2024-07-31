using AutoMapper;
using wca.share.application.Common;
using wca.share.application.Features.TiposFaturamento.Commands;
using wca.share.domain.Entities;

namespace wca.share.application.Features.TiposFaturamento.Common
{
    internal class TipoFaturamentoProfile : Profile
    {
        public TipoFaturamentoProfile()
        {
            CreateMap<TipoFaturamento, TipoFaturamentoResponse>();
            CreateMap<TipoFaturamentoCreateCommand, TipoFaturamento>();
            CreateMap<TipoFaturamentoUpdateCommand, TipoFaturamento>();
            CreateMap<TipoFaturamento, ListItem>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));
        }
    }
}
