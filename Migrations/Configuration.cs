
namespace DinarakProject01.Migrations
{
    using DinarakProject01.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DinarakProject01.Models.DinarakDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DinarakProject01.Models.DinarakDbContext context)
        {
            //var dinarakClass = new List<DinarakClass>
            //{
            // new DinarakClass{UserName="zaid" ,Password="12345678"},
            // new DinarakClass{UserName="noooooo" ,Password="yesssss"},
            // new DinarakClass{UserName="Okayyy" ,Password="okkkkkkkk"},
            // new DinarakClass{UserName="YARA iHAB nIMER" ,Password="YAYAYAYAYAY"},
            // new DinarakClass{UserName="asal01" ,Password="qais123455"},
            // new DinarakClass{UserName="awad" ,Password="awad123455"},
            //};
            //dinarakClass.ForEach(d=> context.dinarakClasses.Add(d));
            //context.SaveChanges();
        }
    }
}
