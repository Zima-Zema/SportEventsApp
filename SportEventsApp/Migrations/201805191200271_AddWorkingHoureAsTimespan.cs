namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWorkingHoureAsTimespan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stores", "WorkingHours", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stores", "WorkingHours");
        }
    }
}
