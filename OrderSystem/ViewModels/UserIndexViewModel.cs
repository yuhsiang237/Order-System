using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSystem.ViewModels
{

    public class UserIndexViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }

}
