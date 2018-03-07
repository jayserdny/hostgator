namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RetryTipDocuments1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TipsCategory", "TipDocument_Id", c => c.Int());
            CreateIndex("dbo.TipsCategory", "TipDocument_Id");
            AddForeignKey("dbo.TipsCategory", "TipDocument_Id", "dbo.TipDocument", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TipsCategory", "TipDocument_Id", "dbo.TipDocument");
            DropIndex("dbo.TipsCategory", new[] { "TipDocument_Id" });
            DropColumn("dbo.TipsCategory", "TipDocument_Id");
        }
    }
}
