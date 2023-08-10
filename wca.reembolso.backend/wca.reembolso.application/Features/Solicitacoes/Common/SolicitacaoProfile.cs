using AutoMapper;
using wca.reembolso.application.Features.Solicitacoes.Commands;
using wca.reembolso.application6.Common;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Solicitacoes.Common
{
    public sealed class SolicitacoProfile : Profile
    {
        public SolicitacoProfile()
        {
            CreateMap<Solicitacao, SolicitacaoResponse>().ReverseMap();
            CreateMap<SolicitacaoCreateCommand, Solicitacao>();
            CreateMap<SolicitacaoUpdateCommand, Solicitacao>();
        }
    }
}
