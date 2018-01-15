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
                        School_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Parent", t => t.Parent_Id)
                .ForeignKey("dbo.School", t => t.School_Id)
                .Index(t => t.Parent_Id)
                .Index(t => t.School_Id);
            
            CreateTable(
                "dbo.TutorNote",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(),
                        SessionNote = c.String(),
                        Student_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Student", t => t.Student_Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.Student_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        PasswordSalt = c.String(nullable: false),
                        PasswordHash = c.String(nullable: false),
                        Active = c.Boolean(nullable: false),
                        LoginErrorMessage = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Address = c.String(),
                        CityStateZip = c.String(),
                        HomePhone = c.String(),
                        CellPhone = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.UserRole",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Role = c.String(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRole", "User_Id", "dbo.User");
            DropForeignKey("dbo.Teacher", "School_Id", "dbo.School");
            DropForeignKey("dbo.TutorNote", "User_Id", "dbo.User");
            DropForeignKey("dbo.TutorNote", "Student_Id", "dbo.Student");
            DropForeignKey("dbo.Student", "School_Id", "dbo.School");
            DropForeignKey("dbo.StudentReport", "Student_Id", "dbo.Student");
            DropForeignKey("dbo.Student", "Parent_Id", "dbo.Parent");
            DropForeignKey("dbo.StudentReport", "DocumentType_Id", "dbo.DocumentType");
            DropIndex("dbo.UserRole", new[] { "User_Id" });
            DropIndex("dbo.Teacher", new[] { "School_Id" });
            DropIndex("dbo.TutorNote", new[] { "User_Id" });
            DropIndex("dbo.TutorNote", new[] { "Student_Id" });
            DropIndex("dbo.Student", new[] { "School_Id" });
            DropIndex("dbo.Student", new[] { "Parent_Id" });
            DropIndex("dbo.StudentReport", new[] { "Student_Id" });
            DropIndex("dbo.StudentReport", new[] { "DocumentType_Id" });
            DropTable("dbo.UserRole");
            DropTable("dbo.Teacher");
            DropTable("dbo.User");
            DropTable("dbo.TutorNote");
            DropTable("dbo.Student");
            DropTable("dbo.StudentReport");
            DropTable("dbo.Staff");
            DropTable("dbo.School");
            DropTable("dbo.Parent");
            DropTable("dbo.DocumentType");
        }
    }
}
