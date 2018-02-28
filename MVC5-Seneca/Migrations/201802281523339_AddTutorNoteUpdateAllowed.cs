namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTutorNoteUpdateAllowed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TutorNote", "UpdateAllowed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TutorNote", "UpdateAllowed");
        }
    }
}
