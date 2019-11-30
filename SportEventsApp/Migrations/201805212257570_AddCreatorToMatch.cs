namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreatorToMatch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Matches", "CreatorId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Matches", "Type", c => c.String());
            CreateIndex("dbo.Matches", "CreatorId");
            AddForeignKey("dbo.Matches", "CreatorId", "dbo.AspNetUsers", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Matches", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.Matches", new[] { "CreatorId" });
            AlterColumn("dbo.Matches", "Type", c => c.Int());
            DropColumn("dbo.Matches", "CreatorId");
        }
    }
}
