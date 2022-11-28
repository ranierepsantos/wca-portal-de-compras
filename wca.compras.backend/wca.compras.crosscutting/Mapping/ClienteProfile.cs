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
            CreateMap<CreateClienteDto,Cliente>()
                .ForMember(src => src.ClienteOrcamentoConfiguracao, dest => dest.Ignore())
                .ForMember(src => src.ClienteContatos, dest => dest.Ignore());

        }
    }
}
