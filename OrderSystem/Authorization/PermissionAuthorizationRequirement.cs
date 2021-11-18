using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSystem.Authorization
{
    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        public string[] Permissions { get; set; }

        public PermissionAuthorizationRequirement(string[] permissions)
        {
            Permissions = permissions;
        }
    }
}
