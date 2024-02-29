using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using urban_trader_be.Model;

namespace urban_trader_be.Data
{
    public class AppDatabaseContext:DbContext
    {
        public AppDatabaseContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {
            
        }
        public DbSet<Stock> Stock{get; set;} //grabbing something from db
        public DbSet<Comment> Comments{get; set;}
    }
}