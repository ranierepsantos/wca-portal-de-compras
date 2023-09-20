using AutoMapper;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Notificacoes.Common
{
    public sealed class NofificacaoProfile: Profile
    {
        public NofificacaoProfile()
        {
            CreateMap<Notificacao, NotificacaoResponse>();
        }
    }
}
