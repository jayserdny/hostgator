namespace MVC5_Seneca.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class RefactorCaseManagerId : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Parent", new[] { "CaseManager_Id" });
            AlterColumn("dbo.Parent", "CaseManager_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Parent", "CaseManager_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Parent", new[] { "CaseManager_Id" });
            AlterColumn("dbo.Parent", "CaseManager_Id", c => c.Int());
            CreateIndex("dbo.Parent", "CaseManager_Id");
        }
    }
}
