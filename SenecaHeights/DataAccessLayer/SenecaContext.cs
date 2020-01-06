using Microsoft.AspNet.Identity.EntityFramework;
using SenecaHeights.EntityModels;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;

namespace SenecaHeights.DataAccessLayer
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
        public DbSet<School> Schools { get; set; }          
        public DbSet<TutorNote> TutorNotes { get; set; }
        public DbSet<Login> Login { get; set; }                
        public static SenecaContext Create()
        {
            return new SenecaContext();
        }                                                                                                                                                                      
        public System.Data.Entity.DbSet<SenecaHeights.EntityModels.TipsCategory> TipsCategories { get; set; }
        public System.Data.Entity.DbSet<SenecaHeights.EntityModels.TipDocument> TipDocuments { get; set; } 
        public System.Data.Entity.DbSet<SenecaHeights.EntityModels.AssociateTutor> AssociateTutors { get; set; }
        public object AssociateTutor { get; internal set; }

        public System.Data.Entity.DbSet<SenecaHeights.EntityModels.TutorSchedule> TutorSchedules { get; set; }
        public System.Data.Entity.DbSet<SenecaHeights.EntityModels.HfedClient> HfedClients { get; set; }
        public System.Data.Entity.DbSet<SenecaHeights.EntityModels.HfedLocation> HfedLocations { get; set; }

        public System.Data.Entity.DbSet<SenecaHeights.EntityModels.HfedProvider> HfedProviders { get; set; }

        public System.Data.Entity.DbSet<SenecaHeights.EntityModels.HfedSchedule> HfedSchedules { get; set; }
    }
}