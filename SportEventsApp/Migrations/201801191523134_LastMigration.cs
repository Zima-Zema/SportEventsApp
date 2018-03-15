namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LastMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Groups", "Event_ID", "dbo.Events");
            DropIndex("dbo.Groups", new[] { "Event_ID" });
            AlterColumn("dbo.Groups", "Event_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.Groups", "Event_ID");
            AddForeignKey("dbo.Groups", "Event_ID", "dbo.Events", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "Event_ID", "dbo.Events");
            DropIndex("dbo.Groups", new[] { "Event_ID" });
            AlterColumn("dbo.Groups", "Event_ID", c => c.Int());
            CreateIndex("dbo.Groups", "Event_ID");
            AddForeignKey("dbo.Groups", "Event_ID", "dbo.Events", "Id");
        }
    }
}
