using AutoMapper;
using wca.share.application.Common;
using wca.share.application.Features.Supervisors.Commands;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Supervisors.Common;

internal class SupervisorProfile : Profile
{
    public SupervisorProfile()
    {
        CreateMap<Supervisor, SupervisorResponse>();
        CreateMap<SupervisorCreateCommand, Supervisor>();
        CreateMap<SupervisorUpdateCommand, Supervisor>();
        CreateMap<Supervisor, ListItem>()
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));
    }
}
