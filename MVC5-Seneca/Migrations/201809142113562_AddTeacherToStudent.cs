namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTeacherToStudent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Student", "Teacher_Id", c => c.Int());
            CreateIndex("dbo.Student", "Teacher_Id");
            AddForeignKey("dbo.Student", "Teacher_Id", "dbo.Teacher", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Student", "Teacher_Id", "dbo.Teacher");
            DropIndex("dbo.Student", new[] { "Teacher_Id" });
            DropColumn("dbo.Student", "Teacher_Id");
        }
    }
}
