namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCashWithRelationwithEvent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EtisalatCashes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        Event_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Event_ID, cascadeDelete: true)
                .Index(t => t.Event_ID);
            
            CreateTable(
                "dbo.VodafoneCashes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        Event_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Event_ID, cascadeDelete: true)
                .Index(t => t.Event_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VodafoneCashes", "Event_ID", "dbo.Events");
            DropForeignKey("dbo.EtisalatCashes", "Event_ID", "dbo.Events");
            DropIndex("dbo.VodafoneCashes", new[] { "Event_ID" });
            DropIndex("dbo.EtisalatCashes", new[] { "Event_ID" });
            DropTable("dbo.VodafoneCashes");
            DropTable("dbo.EtisalatCashes");
        }
    }
}
