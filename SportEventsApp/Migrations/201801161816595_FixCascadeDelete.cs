namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixCascadeDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EtisalatCashes", "Event_ID", "dbo.Events");
            DropForeignKey("dbo.Groups", "Event_ID", "dbo.Events");
            DropForeignKey("dbo.VodafoneCashes", "Event_ID", "dbo.Events");
            DropIndex("dbo.EtisalatCashes", new[] { "Event_ID" });
            DropIndex("dbo.Groups", new[] { "Event_ID" });
            DropIndex("dbo.VodafoneCashes", new[] { "Event_ID" });
            AlterColumn("dbo.EtisalatCashes", "Event_ID", c => c.Int());
            AlterColumn("dbo.Groups", "Event_ID", c => c.Int());
            AlterColumn("dbo.VodafoneCashes", "Event_ID", c => c.Int());
            CreateIndex("dbo.EtisalatCashes", "Event_ID");
            CreateIndex("dbo.Groups", "Event_ID");
            CreateIndex("dbo.VodafoneCashes", "Event_ID");
            AddForeignKey("dbo.EtisalatCashes", "Event_ID", "dbo.Events", "Id");
            AddForeignKey("dbo.Groups", "Event_ID", "dbo.Events", "Id");
            AddForeignKey("dbo.VodafoneCashes", "Event_ID", "dbo.Events", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VodafoneCashes", "Event_ID", "dbo.Events");
            DropForeignKey("dbo.Groups", "Event_ID", "dbo.Events");
            DropForeignKey("dbo.EtisalatCashes", "Event_ID", "dbo.Events");
            DropIndex("dbo.VodafoneCashes", new[] { "Event_ID" });
            DropIndex("dbo.Groups", new[] { "Event_ID" });
            DropIndex("dbo.EtisalatCashes", new[] { "Event_ID" });
            AlterColumn("dbo.VodafoneCashes", "Event_ID", c => c.Int(nullable: false));
            AlterColumn("dbo.Groups", "Event_ID", c => c.Int(nullable: false));
            AlterColumn("dbo.EtisalatCashes", "Event_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.VodafoneCashes", "Event_ID");
            CreateIndex("dbo.Groups", "Event_ID");
            CreateIndex("dbo.EtisalatCashes", "Event_ID");
            AddForeignKey("dbo.VodafoneCashes", "Event_ID", "dbo.Events", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Groups", "Event_ID", "dbo.Events", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EtisalatCashes", "Event_ID", "dbo.Events", "Id", cascadeDelete: true);
        }
    }
}
