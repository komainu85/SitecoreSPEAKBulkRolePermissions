using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MikeRobbins.RolePermissions.Entities
{
    public class PermissionsJson
    {
        public Permission[] permissions { get; set; }
    }

    public class Permission
    {
        public string read { get; set; }
        public string write { get; set; }
        public string rename { get; set; }
        public string create { get; set; }
        public string delete { get; set; }
        public string admin { get; set; }
        public string inheritance { get; set; }
    }
}