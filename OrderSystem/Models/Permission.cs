using System;
using System.Collections.Generic;

#nullable disable

namespace OrderSystem.Models
{
    public partial class Permission
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public string Code { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
