namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActiveRole : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Active");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Active", c => c.Boolean(nullable: false));
        }
    }
}
