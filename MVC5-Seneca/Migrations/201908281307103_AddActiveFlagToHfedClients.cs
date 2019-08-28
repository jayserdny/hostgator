namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActiveFlagToHfedClients : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HfedClient", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HfedClient", "Active");
        }
    }
}
