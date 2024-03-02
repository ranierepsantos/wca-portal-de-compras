using AutoMapper;
using wca.reembolso.application.Features.Despesas.Command;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Despesas.Common
{
    internal class DespesaProfile: Profile
    {
        public DespesaProfile()
        {
            CreateMap<DespesaCreateCommand,Despesa>();
            CreateMap<DespesaUpdateCommand,Despesa>();
        }
    }
}
