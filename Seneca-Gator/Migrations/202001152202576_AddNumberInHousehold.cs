namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNumberInHousehold : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HfedClient", "NumberInHousehold", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HfedClient", "NumberInHousehold");
        }
    }
}
