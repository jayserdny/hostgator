namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAssociateTutors : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssociateTutor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
            DropForeignKey("dbo.AssociateTutor", "Tutor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AssociateTutor", "Student_Id", "dbo.Student");
            DropIndex("dbo.AssociateTutor", new[] { "Tutor_Id" });
            DropIndex("dbo.AssociateTutor", new[] { "Student_Id" });
            DropTable("dbo.AssociateTutor");
        }
    }
}
