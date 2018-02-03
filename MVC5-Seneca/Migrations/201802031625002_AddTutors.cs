namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTutors : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Student", "PrimaryTutor_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Student_Id", c => c.Int());
            CreateIndex("dbo.Student", "PrimaryTutor_Id");
            CreateIndex("dbo.AspNetUsers", "Student_Id");
            AddForeignKey("dbo.Student", "PrimaryTutor_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Student_Id", "dbo.Student", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Student_Id", "dbo.Student");
            DropForeignKey("dbo.Student", "PrimaryTutor_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "Student_Id" });
            DropIndex("dbo.Student", new[] { "PrimaryTutor_Id" });
            DropColumn("dbo.AspNetUsers", "Student_Id");
            DropColumn("dbo.Student", "PrimaryTutor_Id");
        }
    }
}
