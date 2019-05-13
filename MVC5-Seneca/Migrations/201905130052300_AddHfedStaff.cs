namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHfedStaff : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.HfedStaff",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            FirstName = c.String(),
            //            LastName = c.String(),
            //            Phone = c.String(),
            //            Email = c.String(),
            //            StaffNote = c.String(),
            //            Location_Id = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.HfedLocation", t => t.Location_Id, cascadeDelete: true)
            //    .Index(t => t.Location_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HfedStaff", "Location_Id", "dbo.HfedLocation");
            DropIndex("dbo.HfedStaff", new[] { "Location_Id" });
            DropTable("dbo.HfedStaff");
        }
    }
}
