using AutoMapper;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Configuracoes.Common
{
    public sealed class ConfiguracaoProfile : Profile
    {
        public ConfiguracaoProfile()
        {
            CreateMap<Configuracao, ConfiguracaoResponse>();
        }
    }
}
