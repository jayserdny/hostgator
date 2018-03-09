namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUserLocationToTitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Title", c => c.String());
            DropColumn("dbo.AspNetUsers", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Location", c => c.String());
            DropColumn("dbo.AspNetUsers", "Title");
        }
    }
}
