namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDriverToSchedule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HfedSchedule", "Driver_Id", c => c.String(maxLength: 128));
            //CreateIndex("dbo.HfedSchedule", "Driver_Id");
            //AddForeignKey("dbo.HfedSchedule", "Driver_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.HfedSchedule", "Driver_Id", "dbo.AspNetUsers");
            //DropIndex("dbo.HfedSchedule", new[] { "Driver_Id" });
            DropColumn("dbo.HfedSchedule", "Driver_Id");
        }
    }
}
