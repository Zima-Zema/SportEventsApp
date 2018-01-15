namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditUserModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "Event_ID", "dbo.Events");
            DropForeignKey("dbo.Users", "Group_ID", "dbo.Groups");
            DropIndex("dbo.Users", new[] { "Group_ID" });
            DropIndex("dbo.Users", new[] { "Event_ID" });
            AlterColumn("dbo.Users", "Mobile", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "Group_ID", c => c.Int());
            AlterColumn("dbo.Users", "Event_ID", c => c.Int());
            CreateIndex("dbo.Users", "Group_ID");
            CreateIndex("dbo.Users", "Event_ID");
            AddForeignKey("dbo.Users", "Event_ID", "dbo.Events", "Id");
            AddForeignKey("dbo.Users", "Group_ID", "dbo.Groups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "Group_ID", "dbo.Groups");
            DropForeignKey("dbo.Users", "Event_ID", "dbo.Events");
            DropIndex("dbo.Users", new[] { "Event_ID" });
            DropIndex("dbo.Users", new[] { "Group_ID" });
            AlterColumn("dbo.Users", "Event_ID", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "Group_ID", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "Mobile", c => c.String());
            CreateIndex("dbo.Users", "Event_ID");
            CreateIndex("dbo.Users", "Group_ID");
            AddForeignKey("dbo.Users", "Group_ID", "dbo.Groups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Users", "Event_ID", "dbo.Events", "Id", cascadeDelete: true);
        }
    }
}
