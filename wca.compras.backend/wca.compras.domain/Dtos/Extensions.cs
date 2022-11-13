using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.compras.domain.Entities;
using static wca.compras.domain.Dtos.PermissionDtos;
using static wca.compras.domain.Dtos.ProfileDtos;

namespace wca.compras.domain.Dtos
{
    public static class Extensions
    {

        public static ProfileDto asDto(this Profile profile)
        {
            return new ProfileDto(
                profile.Id,
                profile.Name,
                profile.Description,
                profile.Active
                );
        }

        public static ProfilePermissionsDto asDto (this ProfilePermissions profile)
        {
            return new ProfilePermissionsDto(
                profile.Id, 
                profile.Name, 
                profile.Description, 
                profile.Active,
                profile.Permissions.Select(p => p.asDto()).ToList()
                );
        }


        public static PermissionDto asDto(this Permission permission)
        {
            return new PermissionDto(
                permission.Id,
                permission.Name,
                permission.InternalName,
                permission.Description
            );
        }
    }
}
