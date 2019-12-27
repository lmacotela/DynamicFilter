using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicFilter.Models
{
    public class Filter
    {
        public int FilterID { get; set; }
        public string Description { get; set; }
        public string Place { get; set; }

        public string Detail { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Enable { get; set; }

        //Relación con las otras tablas
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        public int TypeID { get; set; }
        public Type Type { get; set; }

    }
}