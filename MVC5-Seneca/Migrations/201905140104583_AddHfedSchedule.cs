namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHfedSchedule : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.HfedSchedule",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Date = c.DateTime(nullable: false, storeType: "date"),
            //            PickUpTime = c.String(),
            //            ScheduleNote = c.String(),
            //            Request = c.Boolean(nullable: false),
            //            Complete = c.Boolean(nullable: false),
            //            Location_Id = c.Int(nullable: false),
            //            PointPerson_Id = c.Int(),
            //            Provider_Id = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.HfedLocation", t => t.Location_Id, cascadeDelete: true)
            //    .ForeignKey("dbo.HfedStaff", t => t.PointPerson_Id)
            //    .ForeignKey("dbo.HfedProvider", t => t.Provider_Id, cascadeDelete: true)
            //    .Index(t => t.Location_Id)
            //    .Index(t => t.PointPerson_Id)
            //    .Index(t => t.Provider_Id);

            ////AddColumn("dbo.HfedClient", "HfedSchedule_Id", c => c.Int());
            ////AddColumn("dbo.HfedDriver", "HfedSchedule_Id", c => c.Int());
            ////CreateIndex("dbo.HfedClient", "HfedSchedule_Id");
            ////CreateIndex("dbo.HfedDriver", "HfedSchedule_Id");
            ////AddForeignKey("dbo.HfedClient", "HfedSchedule_Id", "dbo.HfedSchedule", "Id");
            ////AddForeignKey("dbo.HfedDriver", "HfedSchedule_Id", "dbo.HfedSchedule", "Id");

            //CreateTable(
            //    "dbo.HfedScheduleClient",
            //    c => new
            //    {
            //        ScheduleId = c.Int(nullable: false),
            //        ClientId = c.Int(nullable: false),
            //    });

            //CreateTable(
            //    "dbo.HfedScheduleDriver",
            //    c => new
            //    {
            //        ScheduleId = c.Int(nullable: false),
            //        DriverId = c.Int(nullable: false),
            //    });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HfedSchedule", "Provider_Id", "dbo.HfedProvider");
            DropForeignKey("dbo.HfedSchedule", "PointPerson_Id", "dbo.HfedStaff");
            DropForeignKey("dbo.HfedSchedule", "Location_Id", "dbo.HfedLocation");
            //DropForeignKey("dbo.HfedDriver", "HfedSchedule_Id", "dbo.HfedSchedule");
            //DropForeignKey("dbo.HfedClient", "HfedSchedule_Id", "dbo.HfedSchedule");
            DropIndex("dbo.HfedSchedule", new[] { "Provider_Id" });
            DropIndex("dbo.HfedSchedule", new[] { "PointPerson_Id" });
            DropIndex("dbo.HfedSchedule", new[] { "Location_Id" });
            //DropIndex("dbo.HfedDriver", new[] { "HfedSchedule_Id" });
            //DropIndex("dbo.HfedClient", new[] { "HfedSchedule_Id" });
            //DropColumn("dbo.HfedDriver", "HfedSchedule_Id");
            //DropColumn("dbo.HfedClient", "HfedSchedule_Id");
            DropTable("dbo.HfedSchedule");
        }
    }
}
