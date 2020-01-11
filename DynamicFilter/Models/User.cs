using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicFilter.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string User_ { get; set; }
        public int Password { get; set; }

        public bool Enable { get; set; }
    }
}