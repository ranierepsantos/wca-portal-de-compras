using AutoMapper;
using wca.share.application.Common;
using wca.share.application.Features.Funcoes.Commands;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Funcoes.Common
{
    internal class FuncaoProfile : Profile
    {
        public FuncaoProfile()
        {
            CreateMap<Funcao, FuncaoResponse>();
            CreateMap<FuncaoCreateCommand, Funcao>();
            CreateMap<FuncaoUpdateCommand, Funcao>();
            CreateMap<Funcao, ListItem>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));
        }
    }
}
