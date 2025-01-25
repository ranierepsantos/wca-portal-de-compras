using AutoMapper;
using wca.share.application.Common;
using wca.share.application.Features.Funcionarios.Commands;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Funcionarios.Common
{
    internal sealed class FuncionarioProfile : Profile
    {
        public FuncionarioProfile() {
            CreateMap<Funcionario, FuncionarioListItem>()
                    .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome))
                    .ForMember(dest => dest.CentroCustoNome, opt => opt.MapFrom(src => src.CentroCusto.Nome)); ;

            CreateMap<Funcionario, FuncionarioToPaginateResponse>()
                .ForMember(dest => dest.ClienteNome, opt => opt.MapFrom(src => src.Cliente.Nome))
                .ForMember(dest => dest.CentroCustoNome, opt => opt.MapFrom(src => src.CentroCusto.Nome));

            CreateMap<FuncionarioCreateCommand, Funcionario>();
            CreateMap<FuncionarioUpdateCommand, Funcionario>();
            CreateMap<Funcionario, FuncionarioResponse>();

            CreateMap<Funcionario, ListItem>()
                    .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome));
        }
    }
}
