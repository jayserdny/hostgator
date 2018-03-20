namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllowNullableGradeLevel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Student", "GradeLevel", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Student", "GradeLevel", c => c.Int(nullable: false));
        }
    }
}
