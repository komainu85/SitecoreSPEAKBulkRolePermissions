using MikeRobbins.RolePermissions.Entities;

namespace MikeRobbins.RolePermissions.Interfaces
{
    public interface IPermissionManager
    {
        void SetRoleAccess(RoleAccessPermissions roleAccessPermissions);
    }
}