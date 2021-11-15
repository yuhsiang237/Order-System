﻿using System;
using System.Collections.Generic;

#nullable disable

namespace OrderSystem.Models
{
    public partial class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
