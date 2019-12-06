namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStudentActiveFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Student", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Student", "Active");
        }
    }
}
