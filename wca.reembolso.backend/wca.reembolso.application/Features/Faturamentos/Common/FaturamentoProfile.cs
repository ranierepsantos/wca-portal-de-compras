using AutoMapper;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Faturamentos.Common
{
    public sealed class FaturamentoProfile : Profile
    {
        public FaturamentoProfile()
        {
            CreateMap<Faturamento, FaturamentoResponse>();
            CreateMap<FaturamentoItem, FaturamentoItemResponse>();
            CreateMap<Solicitacao, Solicitacao2Faturamento>();
            CreateMap<Faturamento, FaturamentoPaginateResponse>()
                .ForMember(src => src.ClienteNome, dest => dest.MapFrom(d => d.Cliente.Nome));
        }
    }

}
