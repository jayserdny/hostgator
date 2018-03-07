namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Parent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MotherFather = c.String(),
                        FirstName = c.String(),
                        Address = c.String(),
                        HomePhone = c.String(),
                        CellPhone = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.School",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Staff",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Title = c.String(),
                        WorkPhone = c.String(),
                        CellPhone = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StudentReport",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocumentDate = c.DateTime(),
                        Comments = c.String(),
                        DocumentLink = c.String(),
                        DocumentType_Id = c.Int(),
                        Student_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocumentType", t => t.DocumentType_Id)
                .ForeignKey("dbo.Student", t => t.Student_Id)
                .Index(t => t.DocumentType_Id)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.Student",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        Gender = c.String(),
                        BirthDate = c.DateTime(),
                        Parent_Id = c.Int(),
                        PrimaryTutor_Id = c.String(maxLength: 128),
                        School_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Parent", t => t.Parent_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.PrimaryTutor_Id)
                .ForeignKey("dbo.School", t => t.School_Id)
                .Index(t => t.Parent_Id)
                .Index(t => t.PrimaryTutor_Id)
                .Index(t => t.School_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Location = c.String(),
                        Email = c.String(maxLength: 256),
                        UserName = c.String(nullable: false, maxLength: 256),
                        PhoneNumber = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        Student_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Student", t => t.Student_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.TutorNote",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(),
                        SessionNote = c.String(),
                        UpdateAllowed = c.Boolean(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        Student_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Student", t => t.Student_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.Teacher",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastName = c.String(),
                        FirstName = c.String(),
                        WorkPhone = c.String(),
                        CellPhone = c.String(),
                        Email = c.String(),
                        School_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.School", t => t.School_Id)
                .Index(t => t.School_Id);
            
            CreateTable(
                "dbo.TipDocument",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DocumentLink = c.String(),
                        Category_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TipsCategory", t => t.Category_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Category_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.TipsCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TipDocument", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TipDocument", "Category_Id", "dbo.TipsCategory");
            DropForeignKey("dbo.Teacher", "School_Id", "dbo.School");
            DropForeignKey("dbo.AspNetUsers", "Student_Id", "dbo.Student");
            DropForeignKey("dbo.TutorNote", "Student_Id", "dbo.Student");
            DropForeignKey("dbo.TutorNote", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Student", "School_Id", "dbo.School");
            DropForeignKey("dbo.StudentReport", "Student_Id", "dbo.Student");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Student", "PrimaryTutor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Student", "Parent_Id", "dbo.Parent");
            DropForeignKey("dbo.StudentReport", "DocumentType_Id", "dbo.DocumentType");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.TipDocument", new[] { "User_Id" });
            DropIndex("dbo.TipDocument", new[] { "Category_Id" });
            DropIndex("dbo.Teacher", new[] { "School_Id" });
            DropIndex("dbo.TutorNote", new[] { "Student_Id" });
            DropIndex("dbo.TutorNote", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Student_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Student", new[] { "School_Id" });
            DropIndex("dbo.Student", new[] { "PrimaryTutor_Id" });
            DropIndex("dbo.Student", new[] { "Parent_Id" });
            DropIndex("dbo.StudentReport", new[] { "Student_Id" });
            DropIndex("dbo.StudentReport", new[] { "DocumentType_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.TipsCategory");
            DropTable("dbo.TipDocument");
            DropTable("dbo.Teacher");
            DropTable("dbo.TutorNote");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Student");
            DropTable("dbo.StudentReport");
            DropTable("dbo.Staff");
            DropTable("dbo.School");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Parent");
            DropTable("dbo.DocumentType");
        }
    }
}
