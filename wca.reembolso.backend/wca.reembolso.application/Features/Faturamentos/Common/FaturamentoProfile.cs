using AutoMapper;
using System.Net;
using wca.reembolso.application.Features.Faturamentos.Commands;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Faturamentos.Common
{
    public sealed class FaturamentoProfile : Profile
    {
        public FaturamentoProfile()
        {
            CreateMap<Faturamento, FaturamentoResponse>();
            CreateMap<FaturamentoItem, FaturamentoItemResponse>();
            CreateMap<FaturamentoResponse, Faturamento>().ForMember(src => src.FaturamentoItem, dest => dest.Ignore());

            CreateMap<Solicitacao, Solicitacao2Faturamento>();
            
            CreateMap<Faturamento, FaturamentoPaginateResponse>()
                .ForMember(src => src.ClienteNome, dest => dest.MapFrom(d => d.Cliente.Nome));
            
            CreateMap<FaturamentoCreateCommand, Faturamento>()
                .ForMember(src => src.Status, dest =>  dest.Ignore());
            
            CreateMap<FaturamentoItemCreate, FaturamentoItem>();
        }
    }

}
