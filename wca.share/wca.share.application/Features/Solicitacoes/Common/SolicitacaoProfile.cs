using AutoMapper;
using wca.share.application.Features.Solicitacoes.Commands;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Solicitacoes.Common
{
    internal class SolicitacaoProfile : Profile
    {
        public SolicitacaoProfile()
        {
            CreateMap<Solicitacao, SolicitacaoResponse>();
            CreateMap<SolicitacaoCreateCommand, Solicitacao>()
                .ForMember(src => src.Status, dest => dest.Ignore());
            CreateMap<SolicitacaoUpdateCommand, Solicitacao>()
                .ForMember(src => src.Anexos, dest => dest.Ignore());
        }
    }
}
