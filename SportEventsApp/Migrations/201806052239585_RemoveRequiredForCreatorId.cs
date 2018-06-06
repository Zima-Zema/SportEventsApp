namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRequiredForCreatorId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Matches", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.Matches", new[] { "CreatorId" });
            AlterColumn("dbo.Matches", "CreatorId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Matches", "CreatorId");
            AddForeignKey("dbo.Matches", "CreatorId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Matches", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.Matches", new[] { "CreatorId" });
            AlterColumn("dbo.Matches", "CreatorId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Matches", "CreatorId");
            AddForeignKey("dbo.Matches", "CreatorId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
