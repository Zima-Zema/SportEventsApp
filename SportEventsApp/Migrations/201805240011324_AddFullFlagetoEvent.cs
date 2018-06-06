namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFullFlagetoEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Full", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "Full");
        }
    }
}
