using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Web;


namespace DinarakProject01.Models
{
    public class DinarakDbContext:DbContext
    {
     public DinarakDbContext():base("DinarakProjectConnectionString"){ 
     }
        public DbSet<DinarakClass>dinarakClasses { get; set; }
        public DbSet<BookClass> bookClasses { get; set; }
        
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DinarakClass>()
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<BookClass>()
               .Property(e => e.SerialNumber)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

    }
}