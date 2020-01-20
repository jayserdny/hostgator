namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reset : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.AssociateTutor",
            //    c => new
            //    {
            //        Id = c.Int(nullable: false, identity: true),
            //        Student_Id = c.Int(),
            //        Tutor_Id = c.String(maxLength: 128),
            //    })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Student", t => t.Student_Id)
            //    .ForeignKey("dbo.AspNetUsers", t => t.Tutor_Id)
            //    .Index(t => t.Student_Id)
            //    .Index(t => t.Tutor_Id);

            //CreateTable(
            //    "dbo.Student",
            //    c => new
            //    {
            //        Id = c.Int(nullable: false, identity: true),
            //        FirstName = c.String(),
            //        Gender = c.String(),
            //        BirthDate = c.DateTime(),
            //        GradeLevel = c.Int(),
            //        SpecialClass = c.Boolean(nullable: false),
            //        PrimaryTutor_Id = c.String(maxLength: 128),
            //        Parent_Id = c.Int(),
            //        School_Id = c.Int(),
            //        Teacher_Id = c.Int(),
            //    })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.AspNetUsers", t => t.PrimaryTutor_Id)
            //    .ForeignKey("dbo.Parent", t => t.Parent_Id)
            //    .ForeignKey("dbo.School", t => t.School_Id)
            //    .ForeignKey("dbo.Teacher", t => t.Teacher_Id)
            //    .Index(t => t.PrimaryTutor_Id)
            //    .Index(t => t.Parent_Id)
            //    .Index(t => t.School_Id)
            //    .Index(t => t.Teacher_Id);

            //CreateTable(
            //    "dbo.AspNetUsers",
            //    c => new
            //    {
            //        Id = c.String(nullable: false, maxLength: 128),
            //        FirstName = c.String(),
            //        LastName = c.String(),
            //        Title = c.String(),
            //        Email = c.String(maxLength: 256),
            //        UserName = c.String(nullable: false, maxLength: 256),
            //        PhoneNumber = c.String(),
            //        EmailConfirmed = c.Boolean(nullable: false),
            //        PasswordHash = c.String(),
            //        SecurityStamp = c.String(),
            //        PhoneNumberConfirmed = c.Boolean(nullable: false),
            //        TwoFactorEnabled = c.Boolean(nullable: false),
            //        LockoutEndDateUtc = c.DateTime(),
            //        LockoutEnabled = c.Boolean(nullable: false),
            //        AccessFailedCount = c.Int(nullable: false),
            //        Student_Id = c.Int(),
            //    })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Student", t => t.Student_Id)
            //    .Index(t => t.UserName, unique: true, name: "UserNameIndex")
            //    .Index(t => t.Student_Id);

            //CreateTable(
            //    "dbo.AspNetUserClaims",
            //    c => new
            //    {
            //        Id = c.Int(nullable: false, identity: true),
            //        UserId = c.String(nullable: false, maxLength: 128),
            //        ClaimType = c.String(),
            //        ClaimValue = c.String(),
            //    })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
            //    .Index(t => t.UserId);

            //CreateTable(
            //    "dbo.AspNetUserLogins",
            //    c => new
            //    {
            //        LoginProvider = c.String(nullable: false, maxLength: 128),
            //        ProviderKey = c.String(nullable: false, maxLength: 128),
            //        UserId = c.String(nullable: false, maxLength: 128),
            //    })
            //    .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
            //    .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
            //    .Index(t => t.UserId);

            //CreateTable(
            //    "dbo.AspNetUserRoles",
            //    c => new
            //    {
            //        UserId = c.String(nullable: false, maxLength: 128),
            //        RoleId = c.String(nullable: false, maxLength: 128),
            //    })
            //    .PrimaryKey(t => new { t.UserId, t.RoleId })
            //    .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
            //    .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
            //    .Index(t => t.UserId)
            //    .Index(t => t.RoleId);

            //CreateTable(
            //    "dbo.Parent",
            //    c => new
            //    {
            //        Id = c.Int(nullable: false, identity: true),
            //        MotherFather = c.String(),
            //        FirstName = c.String(),
            //        Address = c.String(),
            //        HomePhone = c.String(),
            //        CellPhone = c.String(),
            //        Email = c.String(),
            //        CaseManager_Id = c.Int(),
            //        CaseManagerUser_Id = c.String(maxLength: 128),
            //    })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Staff", t => t.CaseManager_Id)
            //    .ForeignKey("dbo.AspNetUsers", t => t.CaseManagerUser_Id)
            //    .Index(t => t.CaseManager_Id)
            //    .Index(t => t.CaseManagerUser_Id);

            //CreateTable(
            //    "dbo.Staff",
            //    c => new
            //    {
            //        Id = c.Int(nullable: false, identity: true),
            //        FirstName = c.String(),
            //        LastName = c.String(),
            //        Title = c.String(),
            //        WorkPhone = c.String(),
            //        CellPhone = c.String(),
            //        Email = c.String(),
            //    })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.StudentReport",
            //    c => new
            //    {
            //        Id = c.Int(nullable: false, identity: true),
            //        DocumentDate = c.DateTime(),
            //        Comments = c.String(),
            //        DocumentLink = c.String(),
            //        DocumentType_Id = c.Int(),
            //        Student_Id = c.Int(),
            //    })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.DocumentType", t => t.DocumentType_Id)
            //    .ForeignKey("dbo.Student", t => t.Student_Id)
            //    .Index(t => t.DocumentType_Id)
            //    .Index(t => t.Student_Id);

            //CreateTable(
            //    "dbo.DocumentType",
            //    c => new
            //    {
            //        Id = c.Int(nullable: false, identity: true),
            //        Name = c.String(),
            //    })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.School",
            //    c => new
            //    {
            //        Id = c.Int(nullable: false, identity: true),
            //        Name = c.String(),
            //    })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.Teacher",
            //    c => new
            //    {
            //        Id = c.Int(nullable: false, identity: true),
            //        LastName = c.String(),
            //        FirstName = c.String(),
            //        WorkPhone = c.String(),
            //        CellPhone = c.String(),
            //        Email = c.String(),
            //        School_Id = c.Int(),
            //    })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.School", t => t.School_Id)
            //    .Index(t => t.School_Id);

            //CreateTable(
            //    "dbo.TutorNote",
            //    c => new
            //    {
            //        Id = c.Int(nullable: false, identity: true),
            //        Date = c.DateTime(),
            //        SessionNote = c.String(),
            //        UpdateAllowed = c.Boolean(nullable: false),
            //        ApplicationUser_Id = c.String(maxLength: 128),
            //        Student_Id = c.Int(),
            //    })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
            //    .ForeignKey("dbo.Student", t => t.Student_Id)
            //    .Index(t => t.ApplicationUser_Id)
            //    .Index(t => t.Student_Id);

            //CreateTable(
            //    "dbo.HfedClient",
            //    c => new
            //    {
            //        Id = c.Int(nullable: false, identity: true),
            //        FirstName = c.String(),
            //        LastName = c.String(),
            //        DateOfBirth = c.DateTime(nullable: false, storeType: "date"),
            //        ClientNote = c.String(),
            //        Location_Id = c.Int(nullable: false),
            //    })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.HfedLocation", t => t.Location_Id, cascadeDelete: true)
            //    .Index(t => t.Location_Id);

            CreateTable(
                "dbo.HfedLocation",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    Address = c.String(),
                    MainPhone = c.String(),
                    LocationNote = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.HfedProvider",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    Address = c.String(),
                    MainPhone = c.String(),
                    Fax = c.String(),
                    Email = c.String(),
                    ContactName = c.String(),
                    ContactEmail = c.String(),
                    ContactPhone = c.String(),
                    ProviderNote = c.String(),
                    BoxWeight = c.Decimal(precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.HfedSchedule",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        PickUpTime = c.String(),
                        ScheduleNote = c.String(),
                        Request = c.Boolean(nullable: false),
                        Complete = c.Boolean(nullable: false),
                        HfedDriverIds = c.String(),
                        HfedClientIds = c.String(),
                        VolunteerHours = c.Single(),
                        Location_Id = c.Int(nullable: false),
                        PointPerson_Id = c.String(maxLength: 128),
                        Provider_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HfedLocation", t => t.Location_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.PointPerson_Id)
                .ForeignKey("dbo.HfedProvider", t => t.Provider_Id, cascadeDelete: true)
                .Index(t => t.Location_Id)
                .Index(t => t.PointPerson_Id)
                .Index(t => t.Provider_Id);

            //CreateTable(
            //    "dbo.Login",
            //    c => new
            //    {
            //        Id = c.Int(nullable: false, identity: true),
            //        UserName = c.String(),
            //        FirstName = c.String(),
            //        LastName = c.String(),
            //        DateTime = c.DateTime(nullable: false),
            //        Status = c.String(),
            //    })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.AspNetRoles",
            //    c => new
            //    {
            //        Id = c.String(nullable: false, maxLength: 128),
            //        Name = c.String(nullable: false, maxLength: 256),
            //        Discriminator = c.String(nullable: false, maxLength: 128),
            //    })
            //    .PrimaryKey(t => t.Id)
            //    .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            //CreateTable(
            //    "dbo.TipDocument",
            //    c => new
            //    {
            //        Id = c.Int(nullable: false, identity: true),
            //        Name = c.String(),
            //        DocumentLink = c.String(),
            //        Category_Id = c.Int(),
            //        User_Id = c.String(maxLength: 128),
            //    })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.TipsCategory", t => t.Category_Id)
            //    .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
            //    .Index(t => t.Category_Id)
            //    .Index(t => t.User_Id);

            //CreateTable(
            //    "dbo.TipsCategory",
            //    c => new
            //    {
            //        Id = c.Int(nullable: false, identity: true),
            //        Name = c.String(),
            //    })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.TutorSchedule",
            //    c => new
            //    {
            //        Id = c.Int(nullable: false, identity: true),
            //        DayOfWeekIndex = c.Int(nullable: false),
            //        MinutesPastMidnight = c.Int(nullable: false),
            //        Student_Id = c.Int(),
            //        Tutor_Id = c.String(maxLength: 128),
            //    })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Student", t => t.Student_Id)
            //    .ForeignKey("dbo.AspNetUsers", t => t.Tutor_Id)
            //    .Index(t => t.Student_Id)
            //    .Index(t => t.Tutor_Id);

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TutorSchedule", "Tutor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TutorSchedule", "Student_Id", "dbo.Student");
            DropForeignKey("dbo.TipDocument", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TipDocument", "Category_Id", "dbo.TipsCategory");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.HfedSchedule", "Provider_Id", "dbo.HfedProvider");
            DropForeignKey("dbo.HfedSchedule", "PointPerson_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.HfedSchedule", "Location_Id", "dbo.HfedLocation");
            DropForeignKey("dbo.HfedClient", "Location_Id", "dbo.HfedLocation");
            DropForeignKey("dbo.AssociateTutor", "Tutor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AssociateTutor", "Student_Id", "dbo.Student");
            DropForeignKey("dbo.TutorNote", "Student_Id", "dbo.Student");
            DropForeignKey("dbo.TutorNote", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Student", "Teacher_Id", "dbo.Teacher");
            DropForeignKey("dbo.Teacher", "School_Id", "dbo.School");
            DropForeignKey("dbo.Student", "School_Id", "dbo.School");
            DropForeignKey("dbo.StudentReport", "Student_Id", "dbo.Student");
            DropForeignKey("dbo.StudentReport", "DocumentType_Id", "dbo.DocumentType");
            DropForeignKey("dbo.Student", "Parent_Id", "dbo.Parent");
            DropForeignKey("dbo.Parent", "CaseManagerUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Parent", "CaseManager_Id", "dbo.Staff");
            DropForeignKey("dbo.AspNetUsers", "Student_Id", "dbo.Student");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Student", "PrimaryTutor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.TutorSchedule", new[] { "Tutor_Id" });
            DropIndex("dbo.TutorSchedule", new[] { "Student_Id" });
            DropIndex("dbo.TipDocument", new[] { "User_Id" });
            DropIndex("dbo.TipDocument", new[] { "Category_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.HfedSchedule", new[] { "Provider_Id" });
            DropIndex("dbo.HfedSchedule", new[] { "PointPerson_Id" });
            DropIndex("dbo.HfedSchedule", new[] { "Location_Id" });
            DropIndex("dbo.HfedClient", new[] { "Location_Id" });
            DropIndex("dbo.TutorNote", new[] { "Student_Id" });
            DropIndex("dbo.TutorNote", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Teacher", new[] { "School_Id" });
            DropIndex("dbo.StudentReport", new[] { "Student_Id" });
            DropIndex("dbo.StudentReport", new[] { "DocumentType_Id" });
            DropIndex("dbo.Parent", new[] { "CaseManagerUser_Id" });
            DropIndex("dbo.Parent", new[] { "CaseManager_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Student_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Student", new[] { "Teacher_Id" });
            DropIndex("dbo.Student", new[] { "School_Id" });
            DropIndex("dbo.Student", new[] { "Parent_Id" });
            DropIndex("dbo.Student", new[] { "PrimaryTutor_Id" });
            DropIndex("dbo.AssociateTutor", new[] { "Tutor_Id" });
            DropIndex("dbo.AssociateTutor", new[] { "Student_Id" });
            DropTable("dbo.TutorSchedule");
            DropTable("dbo.TipsCategory");
            DropTable("dbo.TipDocument");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Login");
            DropTable("dbo.HfedSchedule");
            DropTable("dbo.HfedProvider");
            DropTable("dbo.HfedLocation");
            DropTable("dbo.HfedClient");
            DropTable("dbo.TutorNote");
            DropTable("dbo.Teacher");
            DropTable("dbo.School");
            DropTable("dbo.DocumentType");
            DropTable("dbo.StudentReport");
            DropTable("dbo.Staff");
            DropTable("dbo.Parent");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Student");
            DropTable("dbo.AssociateTutor");
        }
    }
}
