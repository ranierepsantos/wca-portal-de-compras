using AutoMapper;
using wca.share.application.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Funcionarios.Common
{
    internal sealed class FuncionarioProfile : Profile
    {
        public FuncionarioProfile() {
            CreateMap<Funcionario, ListItem>()
                    .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome));
        }
    }
}
