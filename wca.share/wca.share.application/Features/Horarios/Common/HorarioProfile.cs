using AutoMapper;
using wca.share.application.Common;
using wca.share.application.Features.Horarios.Commands;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Horarios.Common
{
    internal class HorarioProfile : Profile
    {
        public HorarioProfile()
        {
            CreateMap<Horario, HorarioResponse>();
            CreateMap<HorarioCreateCommand, Horario>();
            CreateMap<HorarioUpdateCommand, Horario>();
            CreateMap<Horario, ListItem>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));
        }
    }
}
