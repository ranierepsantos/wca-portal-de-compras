using System.ComponentModel.DataAnnotations;

namespace wca.compras.domain.Dtos
{
    public class PermissionDtos
    {
        public record PermissionDto(string Id, string Name, string InternalName, string Description);

        public record CreatePermissionDto([Required] string Name, [Required] string InternalName, string Description);
        
        public record UpdatePermissionDto([Required] string Name, [Required] string InternalName, string Description);
    }
}
