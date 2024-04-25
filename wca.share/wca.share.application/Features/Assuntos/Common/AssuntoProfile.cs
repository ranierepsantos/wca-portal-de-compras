using AutoMapper;
using wca.share.application.Features.Assuntos.Commands;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Assuntos.Common
{
    internal class AssuntoProfile : Profile
    {
        public AssuntoProfile()
        {
            CreateMap<Assunto, AssuntoResponse>();
            CreateMap<AssuntoCreateCommand, Assunto>();
            CreateMap<AssuntoUpdateCommand, Assunto>();
        }
    }
}
