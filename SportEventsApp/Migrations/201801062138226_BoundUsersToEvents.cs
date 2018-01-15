namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BoundUsersToEvents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Event_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.Users", "Event_ID");
            AddForeignKey("dbo.Users", "Event_ID", "dbo.Events", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "Event_ID", "dbo.Events");
            DropIndex("dbo.Users", new[] { "Event_ID" });
            DropColumn("dbo.Users", "Event_ID");
        }
    }
}
