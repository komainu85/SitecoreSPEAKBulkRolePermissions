using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MikeRobbins.RolePermissions.Interfaces;
using Sitecore.Data.Items;
using Sitecore.Security.AccessControl;
using Sitecore.Security.Accounts;
using MikeRobbins.RolePermissions.Entities;

namespace MikeRobbins.RolePermissions.BusinessLogic
{
    public class PermissionManager : IPermissionManager
    {
        public void SetRoleAccess(RoleAccessPermissions roleAccessPermissions)
        {
            foreach (var item in roleAccessPermissions.Items)
            {
                var accessRules = item.Security.GetAccessRules();

                foreach (var roleAccessPermission in roleAccessPermissions.AccessPermissions)
                {
                    var accessRight = roleAccessPermission.Key;
                    var accessPermission = roleAccessPermission.Value;

                    if (accessPermission == AccessPermission.NotSet)
                    {
                        accessRules.Helper.RemoveExactMatches(roleAccessPermissions.Role, accessRight);
                    }
                    else
                    {
                        accessRules.Helper.AddAccessPermission(roleAccessPermissions.Role, accessRight, PropagationType.Entity, accessPermission);
                    }
                }

                item.Editing.BeginEdit();
                item.Security.SetAccessRules(accessRules);
                item.Editing.EndEdit();
            }
        }
    }
}