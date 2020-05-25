using DynamicFilter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicFilter.DTO
{
    public class Ticket
    {
        public User User { get; set; }
        public Filter Filter { get; set; }
    }
}