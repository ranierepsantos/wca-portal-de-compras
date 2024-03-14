using AutoMapper;
using wca.share.application.Features.Filiais.Command;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Filiais.Common
{
    internal class FilialProfile: Profile
    {
        public FilialProfile() {
            CreateMap<FilialCreateUpdateCommand, Filial>();
        }
    }
}
