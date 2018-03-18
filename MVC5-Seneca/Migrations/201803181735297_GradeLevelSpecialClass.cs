namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GradeLevelSpecialClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Student", "GradeLevel", c => c.Int(nullable: false));
            AddColumn("dbo.Student", "SpecialClass", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Student", "SpecialClass");
            DropColumn("dbo.Student", "GradeLevel");
        }
    }
}
