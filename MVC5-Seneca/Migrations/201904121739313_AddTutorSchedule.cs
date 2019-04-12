namespace MVC5_Seneca.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddTutorSchedule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TutorSchedule",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DayName = c.String(),
                    TimeOfDay = c.String(),
                    Student_Id = c.Int(),
                    Tutor_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Student", t => t.Student_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Tutor_Id)
                .Index(t => t.Student_Id)
                .Index(t => t.Tutor_Id); 
        }

        public override void Down()
        {
            DropForeignKey("dbo.TutorSchedule", "Tutor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TutorSchedule", "Student_Id", "dbo.Student");
            DropIndex("dbo.TutorSchedule", new[] { "Tutor_Id" });
            DropIndex("dbo.TutorSchedule", new[] { "Student_Id" });
            DropTable("dbo.TutorSchedule");
        }
    }
}
