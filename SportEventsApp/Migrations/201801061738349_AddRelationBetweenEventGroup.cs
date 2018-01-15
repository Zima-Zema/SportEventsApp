namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRelationBetweenEventGroup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "Event_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.Groups", "Event_ID");
            AddForeignKey("dbo.Groups", "Event_ID", "dbo.Events", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "Event_ID", "dbo.Events");
            DropIndex("dbo.Groups", new[] { "Event_ID" });
            DropColumn("dbo.Groups", "Event_ID");
        }
    }
}
