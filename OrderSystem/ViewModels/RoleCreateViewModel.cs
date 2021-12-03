using OrderSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSystem.ViewModels
{
    public class RoleCreateViewModel
    {
        public Role Role { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
