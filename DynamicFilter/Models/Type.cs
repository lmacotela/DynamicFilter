using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicFilter.Models
{
    public class Type
    {
        public int TypeID { get; set; }
        public string Name { get; set; }
        public bool Enable { get; set; }
    }
}