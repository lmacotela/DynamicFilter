using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DynamicFilter.DTO
{
    public class UserFilterDTO
    {
        public int FilterID { get; set; }

        [Column(TypeName = "ntext")]
        //[MaxLength]
        public string Description { get; set; }
        public string Place { get; set; }

        [Column(TypeName = "ntext")]
        //[MaxLength]
        public string Detail { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public bool Enable { get; set; }
        //Relación con las otras tablas
        public int CategoryID { get; set; }
        public int TypeID { get; set; }
        public int StateID { get; set; }
        public int user
    }
}