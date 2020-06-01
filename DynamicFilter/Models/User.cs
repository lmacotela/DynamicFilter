using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DynamicFilter.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string ProviderName { get; set; }
        public string ContactPerson { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Enable { get; set; }
        public int RoleID { get; set; }
        public Role Role { get; set; }
        public int ProveedorID { get; set; }
    }
}