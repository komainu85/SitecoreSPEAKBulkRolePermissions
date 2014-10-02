using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MikeRobbins.RolePermissions.BusinessLogic;
using MikeRobbins.RolePermissions.Entities;
using MikeRobbins.RolePermissions.Interfaces;
using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Security.AccessControl;
using StructureMap;

namespace MikeRobbins.RolePermissions
{
    public class RolePermissionsController : Controller
    {
        public RolePermissionsController()
        {
            ObjectFactory.Initialize(x => x.For<Interfaces.IPermissionManager>().Singleton().Use<PermissionManager>());
        }

        public string GetAllRoles()
        {
            var roles = Sitecore.Security.Accounts.RolesInRolesManager.GetAllRoles();
            return JsonConvert.SerializeObject(roles);
        }

        public bool ApplyUserPermissions(string role, string selectedItems, string selectedPermissions)
        {
            try
            {
                var roleAccount = Sitecore.Security.Accounts.Role.FromName(role);
                var items = GetItems(selectedItems);
                var permissions = GetPermissions(selectedPermissions);

                var permissionManager = ObjectFactory.GetInstance<IPermissionManager>();

                permissionManager.SetRoleAccess(new RoleAccessPermissions()
                {
                    Role = roleAccount,
                    Items = items,
                    AccessPermissions = permissions
                });

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Item> GetItems(string items)
        {
            var ids = items.Split(new char[] { '|' });

            var master = Sitecore.Data.Database.GetDatabase("master");

            return ids.Select(id => master.GetItem(new ID(id))).ToList();
        }

        public Dictionary<AccessRight, AccessPermission> GetPermissions(string permissions)
        {
            var accessPermissions = new Dictionary<AccessRight, AccessPermission>();

            var permissionsSet = JsonConvert.DeserializeObject<PermissionsJson>(permissions);

            var classType = typeof(Permission);

            foreach (PropertyInfo property in classType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = property.GetValue(permissionsSet.permissions[0]);

                if (value != null)
                {
                    var parsedValue = ParseAccessPermission(value.ToString());
                    var parsedAccessRight = ParseAccessRight(property.Name);

                    if (parsedAccessRight != null)
                    {
                        accessPermissions.Add(parsedAccessRight, parsedValue);
                    }
                }
            }

            return accessPermissions;
        }

        public AccessPermission ParseAccessPermission(string permission)
        {
            var accessPermission = AccessPermission.NotSet;

            if (AccessPermission.TryParse(permission, out accessPermission))
            {
                return accessPermission;
            }
            else
            {
                throw new Exception();
            }

        }

        public AccessRight ParseAccessRight(string accessRight)
        {
            if (!string.IsNullOrEmpty(accessRight))
            {
                return AccessRight.FromName("item:" +accessRight);
            }
            return null;
        }
    }
}