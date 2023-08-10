using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.reembolso.application.Features.Clientes.Commands;
using wca.reembolso.application6.Common;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Clientes.Common
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
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome));
        }
    }
}
