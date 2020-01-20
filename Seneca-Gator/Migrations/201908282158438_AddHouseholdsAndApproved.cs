namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHouseholdsAndApproved : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HfedSchedule", "Approved", c => c.Boolean(nullable: false));
            AddColumn("dbo.HfedSchedule", "Households", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HfedSchedule", "Households");
            DropColumn("dbo.HfedSchedule", "Approved");
        }
    }
}
