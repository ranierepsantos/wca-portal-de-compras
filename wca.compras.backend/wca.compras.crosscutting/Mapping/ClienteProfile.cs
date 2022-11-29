using AutoMapper;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;

namespace wca.compras.crosscutting.Mapping
{
    public class ClienteProfile: Profile
    {
        public ClienteProfile()
        {
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            //    .ForMember(src => src.ClienteContatos, dest => dest.MapFrom(opt => opt.ClienteContatos))
            //    .ForMember(src => src.ClienteOrcamentoConfiguracao, dest => dest.MapFrom(opt => opt.ClienteOrcamentoConfiguracao))
            //    .ReverseMap();
            CreateMap<CreateClienteDto,Cliente>()
                .ForMember(src => src.ClienteOrcamentoConfiguracao, dest => dest.Ignore())
                .ForMember(src => src.ClienteContatos, dest => dest.Ignore());
            CreateMap<UpdateClienteDto, Cliente>()
                .ForMember(src => src.ClienteOrcamentoConfiguracao, dest => dest.MapFrom(opt => opt.ClienteOrcamentoConfiguracao))
                .ForMember(src => src.ClienteContatos, dest => dest.MapFrom(opt => opt.ClienteContatos));
            CreateMap<ClienteContato, ClienteContatoDto>().ReverseMap();
            CreateMap<ClienteOrcamentoConfiguracao, ClienteOrcamentoConfiguracaoDto>().ReverseMap();
        }
    }
}
