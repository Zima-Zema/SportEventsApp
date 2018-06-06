namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixRelationUserStore : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Stores", "OwnerId", c => c.String(maxLength: 128));
            //CreateIndex("dbo.Stores", "OwnerId");
            //AddForeignKey("dbo.Stores", "OwnerId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stores", "OwnerId", "dbo.AspNetUsers");
            DropIndex("dbo.Stores", new[] { "OwnerId" });
            DropColumn("dbo.Stores", "OwnerId");
        }
    }
}
