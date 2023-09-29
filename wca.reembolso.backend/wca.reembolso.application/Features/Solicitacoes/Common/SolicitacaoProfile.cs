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
            CreateMap<SolicitacaoResponse, Solicitacao>()
                .ForMember(src => src.Colaborador, dest => dest.Ignore())
                .ForMember(src => src.Gestor, dest => dest.Ignore())
                .ForMember(src => src.Cliente, dest => dest.Ignore());
            CreateMap<SolicitacaoCreateCommand, Solicitacao>();
            CreateMap<SolicitacaoUpdateCommand, Solicitacao>();
        }
    }
}
