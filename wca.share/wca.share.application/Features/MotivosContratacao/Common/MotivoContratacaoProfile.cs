using AutoMapper;
using wca.share.application.Common;
using wca.share.application.Features.MotivosContratacao.Commands;
using wca.share.domain.Entities;

namespace wca.share.application.Features.MotivosContratacao.Common
{
    internal class MotivoContratacaoProfile : Profile
    {
        public MotivoContratacaoProfile()
        {
            CreateMap<MotivoContratacao, MotivoContratacaoResponse>();
            CreateMap<MotivoContratacaoCreateCommand, MotivoContratacao>();
            CreateMap<MotivoContratacaoUpdateCommand, MotivoContratacao>();
            CreateMap<MotivoContratacao, ListItem>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));
        }
    }
}
