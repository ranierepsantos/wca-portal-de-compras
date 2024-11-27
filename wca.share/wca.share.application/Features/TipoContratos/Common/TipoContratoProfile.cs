using AutoMapper;
using wca.share.application.Common;
using wca.share.application.Features.TipoContratos.Commands;
using wca.share.domain.Entities;

namespace wca.share.application.Features.TipoContratos.Common
{
    internal class TipoContratoProfile : Profile
    {
        public TipoContratoProfile()
        {
            CreateMap<TipoContrato, TipoContratoResponse>();
            CreateMap<TipoContratoCreateCommand, TipoContrato>();
            CreateMap<TipoContratoUpdateCommand, TipoContrato>();
            CreateMap<TipoContrato, ListItem>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));
        }
    }
}
