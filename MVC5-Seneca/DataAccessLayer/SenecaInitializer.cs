using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MVC5_Seneca.EntityModels;

namespace MVC5_Seneca.DataAccessLayer
{

    public class SenecaInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SenecaContext>
    {
        //protected override void Seed
        //{
        //    new User { name = "P", password = "p" }
        //};
        //Context.SaveChanges();
    }
}