namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMainPhoneToHfedLcation : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.HfedLocation", "MainPhone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HfedLocation", "MainPhone");
        }
    }
}
