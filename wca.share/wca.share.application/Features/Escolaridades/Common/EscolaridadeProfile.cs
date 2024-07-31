using AutoMapper;
using wca.share.application.Common;
using wca.share.application.Features.Escolaridades.Commands;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Escolaridades.Common
{
    internal class EscolaridadeProfile : Profile
    {
        public EscolaridadeProfile()
        {
            CreateMap<Escolaridade, EscolaridadeResponse>();
            CreateMap<EscolaridadeCreateCommand, Escolaridade>();
            CreateMap<EscolaridadeUpdateCommand, Escolaridade>();
            CreateMap<Escolaridade, ListItem>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));
        }
    }
}
