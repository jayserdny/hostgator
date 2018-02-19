using Microsoft.AspNet.Identity.EntityFramework;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;

namespace MVC5_Seneca.DataAccessLayer
{
    public class SenecaContext : IdentityDbContext <ApplicationUser>
    {
        public static void EnableMigrations()
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<SenecaContext>());
        }
        public static void DropAndCreateDatabase()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<SenecaContext>());
        }
        public SenecaContext() : base("SenecaContext")
        {
            Database.Log = msg => Debug.Write(msg);
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Parent> Parents { get; set; } 
        public DbSet<Teacher> Teachers { get; set; }        
        public DbSet<Staff> StaffMembers { get; set; } 
        public DbSet<DocumentType> DocumentTypes { get; set; } 
        public DbSet<StudentReport> StudentReports { get; set; }      

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }   
        public System.Data.Entity.DbSet<MVC5_Seneca.Models.School> Schools { get; set; }        

        public System.Data.Entity.DbSet<MVC5_Seneca.EntityModels.TutorNote> TutorNotes { get; set; }

        public static SenecaContext Create()
        {
            return new SenecaContext();
        }

    }
}