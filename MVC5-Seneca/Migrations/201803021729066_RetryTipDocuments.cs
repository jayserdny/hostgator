namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RetryTipDocuments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TipDocument", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.TipDocument", "User_Id");
            AddForeignKey("dbo.TipDocument", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TipDocument", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.TipDocument", new[] { "User_Id" });
            DropColumn("dbo.TipDocument", "User_Id");
        }
    }
}
