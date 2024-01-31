using AutoMapper;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Notificacoes.Common
{
    public sealed class NofificacaoProfile: Profile
    {
        public NofificacaoProfile()
        {
            CreateMap<Notificacao, NotificacaoResponse>();
        }
    }
}
