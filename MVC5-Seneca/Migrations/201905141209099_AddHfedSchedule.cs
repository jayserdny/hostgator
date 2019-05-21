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
            //    {
            //        Id = c.Int(nullable: false, identity: true),
            //        Date = c.DateTime(nullable: false, storeType: "date"),
            //        PickUpTime = c.String(),
            //        ScheduleNote = c.String(),
            //        Request = c.Boolean(nullable: false),
            //        Complete = c.Boolean(nullable: false),
            //        Location_Id = c.Int(nullable: false),
            //        PointPerson_Id = c.Int(),
            //        Provider_Id = c.Int(nullable: false),
            //        HfedDriverIds = c.String(),
            //        HfedClientIds = c.String(),
            //    })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.HfedLocation", t => t.Location_Id, cascadeDelete: true)
            //    .ForeignKey("dbo.HfedStaff", t => t.PointPerson_Id)
            //    .ForeignKey("dbo.HfedProvider", t => t.Provider_Id, cascadeDelete: true)
            //    .Index(t => t.Location_Id)
            //    .Index(t => t.PointPerson_Id)
            //    .Index(t => t.Provider_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.HfedSchedule", "Provider_Id", "dbo.HfedProvider");
            DropForeignKey("dbo.HfedSchedule", "PointPerson_Id", "dbo.HfedStaff");
            DropForeignKey("dbo.HfedSchedule", "Location_Id", "dbo.HfedLocation");
            DropIndex("dbo.HfedSchedule", new[] { "Provider_Id" });
            DropIndex("dbo.HfedSchedule", new[] { "PointPerson_Id" });
            DropIndex("dbo.HfedSchedule", new[] { "Location_Id" });
            DropTable("dbo.HfedSchedule");
        }
    }
}
