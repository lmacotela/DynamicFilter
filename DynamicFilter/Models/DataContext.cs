using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DynamicFilter.Models
{
    public class DataContext : DbContext
    {
        public DataContext() : base("name= DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Filter> Filters { get; set; }
        
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

    }
}