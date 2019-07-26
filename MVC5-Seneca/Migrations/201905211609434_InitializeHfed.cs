namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitializeHfed : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HfedClient",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false, storeType: "date"),
                        ClientNote = c.String(),
                        Location_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HfedLocation", t => t.Location_Id, cascadeDelete: true)
                .Index(t => t.Location_Id);
            
            CreateTable(
                "dbo.HfedLocation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        MainPhone = c.String(),
                        LocationNote = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HfedProvider",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        MainPhone = c.String(),
                        Fax = c.String(),
                        Email = c.String(),
                        ContactName = c.String(),
                        ContactEmail = c.String(),
                        ContactPhone = c.String(),
                        ProviderNote = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HfedSchedule",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        PickUpTime = c.String(),
                        ScheduleNote = c.String(),
                        Request = c.Boolean(nullable: false),
                        Complete = c.Boolean(nullable: false),
                        HfedDriverIds = c.String(),
                        HfedClientIds = c.String(),
                        Location_Id = c.Int(nullable: false),
                        PointPerson_Id = c.Single( ),
                        Provider_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HfedLocation", t => t.Location_Id, cascadeDelete: true)
                .ForeignKey("dbo.HfedProvider", t => t.Provider_Id, cascadeDelete: true)
                .Index(t => t.Location_Id)
                .Index(t => t.PointPerson_Id)
                .Index(t => t.Provider_Id);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HfedSchedule", "Provider_Id", "dbo.HfedProvider");
            DropForeignKey("dbo.HfedSchedule", "PointPerson_Id", "dbo.HfedStaff");
            DropForeignKey("dbo.HfedStaff", "Location_Id", "dbo.HfedLocation");
            DropForeignKey("dbo.HfedSchedule", "Location_Id", "dbo.HfedLocation");
            DropForeignKey("dbo.HfedClient", "Location_Id", "dbo.HfedLocation");
            DropIndex("dbo.HfedStaff", new[] { "Location_Id" });
            DropIndex("dbo.HfedSchedule", new[] { "Provider_Id" });
            DropIndex("dbo.HfedSchedule", new[] { "PointPerson_Id" });
            DropIndex("dbo.HfedSchedule", new[] { "Location_Id" });
            DropIndex("dbo.HfedClient", new[] { "Location_Id" });
            DropTable("dbo.HfedStaff");
            DropTable("dbo.HfedSchedule");
            DropTable("dbo.HfedProvider");
            DropTable("dbo.HfedDriver");
            DropTable("dbo.HfedLocation");
            DropTable("dbo.HfedClient");
        }
    }
}
