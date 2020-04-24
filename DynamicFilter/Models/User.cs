using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicFilter.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool Enable { get; set; }

        /// <summary>
        /// Hacen referencia  a la llave foránea
        /// </summary>
        public int RoleID { get; set; }
        public Role Role { get; set; }

        //public virtual List<Filter> Filters { get; set; }

    }
}