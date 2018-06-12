namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStaffToParent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Parent", "CaseManager_Id", c => c.Int());
            CreateIndex("dbo.Parent", "CaseManager_Id");
            AddForeignKey("dbo.Parent", "CaseManager_Id", "dbo.Staff", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Parent", "CaseManager_Id", "dbo.Staff");
            DropIndex("dbo.Parent", new[] { "CaseManager_Id" });
            DropColumn("dbo.Parent", "CaseManager_Id");
        }
    }
}
