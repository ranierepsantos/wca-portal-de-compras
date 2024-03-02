using AutoMapper;
using wca.reembolso.application.Features.Solicitacoes.Commands;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Solicitacoes.Common
{
    public sealed class SolicitacoProfile : Profile
    {
        public SolicitacoProfile()
        {
            CreateMap<Solicitacao, SolicitacaoResponse>();
            CreateMap<Solicitacao, SolicitacaoToPaginateResponse>()
                .ForMember(dest =>  dest.DataMenorDespesa, src=> src.MapFrom(x =>  x.Despesa.Min(y=> y.DataEvento)))
                .ForMember(dest => dest.DataMaiorDespesa, src => src.MapFrom(x => x.Despesa.Max(y => y.DataEvento)));
            CreateMap<SolicitacaoResponse, Solicitacao>()
                .ForMember(src => src.Colaborador, dest => dest.Ignore())
                .ForMember(src => src.CentroCusto, dest => dest.Ignore())
                .ForMember(src => src.Cliente, dest => dest.Ignore());
            CreateMap<SolicitacaoCreateCommand, Solicitacao>();
            CreateMap<SolicitacaoUpdateCommand, Solicitacao>();
        }
    }
}
