namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditApplicationUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "Event_ID", "dbo.Events");
            DropIndex("dbo.Users", new[] { "Group_ID" });
            DropIndex("dbo.Users", new[] { "Event_ID" });
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        ArName = c.String(),
                        EnName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EventUsers",
                c => new
                    {
                        EventId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.EventId, t.UserId })
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.EventId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.AspNetUsers", "Mobile", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "Group_ID", c => c.Int());
            AddColumn("dbo.AspNetUsers", "CashNumber", c => c.String());
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            AddColumn("dbo.AspNetUsers", "City_ID", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "PictureUrl", c => c.String());
            CreateIndex("dbo.AspNetUsers", "Group_ID");
            CreateIndex("dbo.AspNetUsers", "City_ID");
            AddForeignKey("dbo.AspNetUsers", "City_ID", "dbo.Cities", "Id");
            DropTable("dbo.Users");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        Mobile = c.String(nullable: false),
                        Status = c.Boolean(nullable: false),
                        Group_ID = c.Int(),
                        Event_ID = c.Int(),
                        CashNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.EventUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "City_ID", "dbo.Cities");
            DropForeignKey("dbo.EventUsers", "EventId", "dbo.Events");
            DropIndex("dbo.AspNetUsers", new[] { "City_ID" });
            DropIndex("dbo.AspNetUsers", new[] { "Group_ID" });
            DropIndex("dbo.EventUsers", new[] { "UserId" });
            DropIndex("dbo.EventUsers", new[] { "EventId" });
            DropColumn("dbo.AspNetUsers", "PictureUrl");
            DropColumn("dbo.AspNetUsers", "City_ID");
            DropColumn("dbo.AspNetUsers", "Address");
            DropColumn("dbo.AspNetUsers", "CashNumber");
            DropColumn("dbo.AspNetUsers", "Group_ID");
            DropColumn("dbo.AspNetUsers", "Mobile");
            DropTable("dbo.EventUsers");
            DropTable("dbo.Cities");
            CreateIndex("dbo.Users", "Event_ID");
            CreateIndex("dbo.Users", "Group_ID");
            AddForeignKey("dbo.Users", "Event_ID", "dbo.Events", "Id");
        }
    }
}
