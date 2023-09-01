using AutoMapper;
using wca.reembolso.application.Features.Conta.Commands;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Conta.Common
{
    internal class ContaCorrenteProfile : Profile
    {
        public ContaCorrenteProfile()
        {
            CreateMap<ContaCorrente, ContaCorrenteResponse>().ReverseMap();
            CreateMap<ContaCorrenteCreateUpdateCommand, ContaCorrente>();
        }
    }
}
