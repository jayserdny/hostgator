namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DisallowPointPersonNulls : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HfedSchedule", "PointPerson_Id", "dbo.AspNetUsers");
            DropIndex("dbo.HfedSchedule", new[] { "PointPerson_Id" });
            AlterColumn("dbo.HfedSchedule", "PointPerson_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.HfedSchedule", "PointPerson_Id");
            AddForeignKey("dbo.HfedSchedule", "PointPerson_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HfedSchedule", "PointPerson_Id", "dbo.AspNetUsers");
            DropIndex("dbo.HfedSchedule", new[] { "PointPerson_Id" });
            AlterColumn("dbo.HfedSchedule", "PointPerson_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.HfedSchedule", "PointPerson_Id");
            AddForeignKey("dbo.HfedSchedule", "PointPerson_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
