using static wca.compras.domain.Dtos.PerfilDtos;

namespace wca.compras.domain.Interfaces.Services
{
    public interface IProfileService
    {
        public Task<ProfileDto> Create(CreateProfileDto profile);

        public Task<ProfileDto> Update(UpdateProfileDto profile);

        public Task<ProfilePermissionsDto> GetProfilePermissions(string id);

    }
}
