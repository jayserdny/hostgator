namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTeachingTips : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TipDocument",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DocumentLink = c.String(),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TipsCategory", t => t.Category_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.TipsCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TipDocument", "Category_Id", "dbo.TipsCategory");
            DropIndex("dbo.TipDocument", new[] { "Category_Id" });
            DropTable("dbo.TipsCategory");
            DropTable("dbo.TipDocument");
        }
    }
}
