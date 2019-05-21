namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHfedDriver : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.HfedDriver",
            //    c => new
            //    {
            //        Id = c.Int(nullable: false, identity: true),
            //        FirstName = c.String(),
            //        LastName = c.String(),
            //        Phone = c.String(),
            //        Fax = c.String(),
            //        Email = c.String(),
            //        DriverNote = c.String(),
            //    })
            //    .PrimaryKey(t => t.Id);

        }
        
        public override void Down()
        {
            DropTable("dbo.HfedDriver");
        }
    }
}
