namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCaseManagerUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Parent", "CaseManagerUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Parent", "CaseManagerUser_Id");
            AddForeignKey("dbo.Parent", "CaseManagerUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Parent", "CaseManagerUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Parent", new[] { "CaseManagerUser_Id" });
            DropColumn("dbo.Parent", "CaseManagerUser_Id");
        }
    }
}
