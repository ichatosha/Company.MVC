using Company.Route.DAL.Data.Configuration;
using Company.Route.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route.DAL.Data.Contexts
{
    public class AppDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new DEpartmentConfigurations());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        // constructor 
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.; Database=CopmanyRoute; Trusted_COnnection = True; TrustServerCertificate = True;");
        //}


        public DbSet<Department> Departments { get; set; }
    }
}
