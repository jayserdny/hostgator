using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MVC5_Seneca.Migrations
{
    using Microsoft.AspNet.Identity;
    using MVC5_Seneca.DataAccessLayer;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MVC5_Seneca.DataAccessLayer.SenecaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MVC5_Seneca.DataAccessLayer.SenecaContext context)
        {
            //  This method will be called after migrating to the latest version.    
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.           
       }
    }
}
