using AutoMapper;
using wca.share.application.Common;
using wca.share.application.Features.Gestors.Commands;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Gestors.Common
{
    internal class GestorProfile : Profile
    {
        public GestorProfile()
        {
            CreateMap<Gestor, GestorResponse>();
            CreateMap<GestorCreateCommand, Gestor>();
            CreateMap<GestorUpdateCommand, Gestor>();
            CreateMap<Gestor, ListItem>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));
        }
    }
}
