using AutoMapper;
using wca.share.application.Common;
using wca.share.application.Features.Escalas.Commands;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Escalas.Common
{
    internal class EscalaProfile : Profile
    {
        public EscalaProfile()
        {
            CreateMap<Escala, EscalaResponse>();
            CreateMap<EscalaCreateCommand, Escala>();
            CreateMap<EscalaUpdateCommand, Escala>();
            CreateMap<Escala, ListItem>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));
        }
    }
}
