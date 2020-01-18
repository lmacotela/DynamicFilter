using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicFilter.Models
{
    public class Role
    {
        public int RoleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enable { get; set; }
    }
}