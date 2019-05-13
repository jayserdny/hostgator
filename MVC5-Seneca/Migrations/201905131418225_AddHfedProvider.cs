namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHfedProvider : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.HfedProvider",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(),
            //            Address = c.String(),
            //            MainPhone = c.String(),
            //            Fax = c.String(),
            //            Email = c.String(),
            //            ContactName = c.String(),
            //            ContactEmail = c.String(),
            //            ContactPhone = c.String(),
            //            ProviderNote = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.HfedProvider");
        }
    }
}
