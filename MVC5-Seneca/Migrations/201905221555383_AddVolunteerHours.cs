namespace MVC5_Seneca.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddVolunteerHours : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HfedSchedule", "VolunteerHours", c => c.Single());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HfedSchedule", "VolunteerHours");
        }
    }
}
