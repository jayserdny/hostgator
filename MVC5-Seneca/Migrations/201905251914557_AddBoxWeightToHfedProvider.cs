namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBoxWeightToHfedProvider : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HfedProvider", "BoxWeight", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HfedProvider", "BoxWeight");
        }
    }
}
