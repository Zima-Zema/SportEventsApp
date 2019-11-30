namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class applyManyToManyEventsStores : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "StoreId", "dbo.Stores");
            DropIndex("dbo.Events", new[] { "StoreId" });
            CreateTable(
                "dbo.EventsStores",
                c => new
                    {
                        EventId = c.Int(nullable: false),
                        StoreId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EventId, t.StoreId })
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.Stores", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.EventId)
                .Index(t => t.StoreId);
            
            AddColumn("dbo.Events", "MatchDuration", c => c.Int(nullable: false));
            DropColumn("dbo.Events", "Host_1");
            DropColumn("dbo.Events", "Host_2");
            DropColumn("dbo.Events", "Host_3");
            DropColumn("dbo.Events", "Match_Duration");
            DropColumn("dbo.Events", "Address");
            DropColumn("dbo.Events", "StoreId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "StoreId", c => c.Int());
            AddColumn("dbo.Events", "Address", c => c.String(nullable: false));
            AddColumn("dbo.Events", "Match_Duration", c => c.Int(nullable: false));
            AddColumn("dbo.Events", "Host_3", c => c.String());
            AddColumn("dbo.Events", "Host_2", c => c.String());
            AddColumn("dbo.Events", "Host_1", c => c.String(nullable: false));
            DropForeignKey("dbo.EventsStores", "StoreId", "dbo.Stores");
            DropForeignKey("dbo.EventsStores", "EventId", "dbo.Events");
            DropIndex("dbo.EventsStores", new[] { "StoreId" });
            DropIndex("dbo.EventsStores", new[] { "EventId" });
            DropColumn("dbo.Events", "MatchDuration");
            DropTable("dbo.EventsStores");
            CreateIndex("dbo.Events", "StoreId");
            AddForeignKey("dbo.Events", "StoreId", "dbo.Stores", "Id");
        }
    }
}
