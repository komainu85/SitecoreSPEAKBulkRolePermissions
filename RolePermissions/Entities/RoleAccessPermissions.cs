using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.ApplicationCenter.Applications;
using Sitecore.Data.Items;
using Sitecore.Security.AccessControl;
using Sitecore.Security.Accounts;

namespace MikeRobbins.RolePermissions.Entities
{
    public class RoleAccessPermissions
    {
        public Role Role { get; set; }
        public List<Item>  Items { get; set; }
        public Dictionary<AccessRight, AccessPermission> AccessPermissions { get; set; }
    }
}