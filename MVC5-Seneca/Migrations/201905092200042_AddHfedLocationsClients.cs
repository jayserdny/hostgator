namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHfedLocationsClients : DbMigration
    {
        public override void Up()
        {
        //    CreateTable(
        //        "dbo.HfedClient",
        //        c => new
        //        {
        //            Id = c.Int(nullable: false, identity: true),
        //            FirstName = c.String(),
        //            LastName = c.String(),
        //            DateOfBirth = c.DateTime(nullable: false, storeType: "date"),
        //            ClientNote = c.String(),
        //            Location_Id = c.Int(nullable: false),
        //        })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.HfedLocation", t => t.Location_Id, cascadeDelete: true)
        //        .Index(t => t.Location_Id);

        //    CreateTable(
        //        "dbo.HfedLocation",
        //        c => new
        //        {
        //            Id = c.Int(nullable: false, identity: true),
        //            Name = c.String(),
        //            Address = c.String(),   
        //            LocationNote = c.String(),
        //            MainPhone = c.String(),        
        //})
        //        .PrimaryKey(t => t.Id);

    }

        public override void Down()
        {
            DropForeignKey("dbo.HfedClient", "Location_Id", "dbo.HfedLocation");
            DropIndex("dbo.HfedClient", new[] { "Location_Id" });
            DropTable("dbo.HfedLocation");
            DropTable("dbo.HfedClient");
        }
    }
}
