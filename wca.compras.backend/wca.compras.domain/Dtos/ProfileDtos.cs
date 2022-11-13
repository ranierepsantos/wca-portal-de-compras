using System.ComponentModel.DataAnnotations;
using static wca.compras.domain.Dtos.PermissionDtos;

namespace wca.compras.domain.Dtos
{
    public class ProfileDtos
    {
        public record ProfileDto (string Id, string Name, string Description, bool Active);
        
        public record ProfilePermissionsDto(string Id, string Name, string Description, bool Active, IList<PermissionDto> Permissions);

        public record CreateProfileDto([Required] string Name, string Description, IList<PermissionDto> Permissions);

        public record UpdateProfileDto([Required] string Name, string Description, bool Active, IList<PermissionDto> Permissions);

    }
}
