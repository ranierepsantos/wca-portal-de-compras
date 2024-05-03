using AutoMapper;
using wca.share.application.Common;
using wca.share.application.Features.Clientes.Commands;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Clientes.Common
{
    public sealed class ClienteProfile : Profile
    {
        public ClienteProfile()
        {
            CreateMap<Cliente, ClienteResponse>();
            CreateMap<ClienteCreateCommand, Cliente>();
            CreateMap<ClienteUpdateCommand, Cliente>();
            CreateMap<Cliente, ListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src =>  (src.CodigoCliente == null ? "": src.CodigoCliente.ToString() + " - ")  + src.Nome));

            CreateMap<Contracts.Integration.GI.Models.ClienteResponse, ClienteCreateCommand>()
                .ForMember(dest => dest.Nome , opt => opt.MapFrom(src => src.RazaoSocial))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.ClienteAtivo))
                .ForMember(dest => dest.CNPJ, opt => opt.MapFrom(src => src.Cgc))
                .ForMember(dest => dest.CNPJ, opt => opt.MapFrom(src => src.Ie))

                ;
        }
    }
}
