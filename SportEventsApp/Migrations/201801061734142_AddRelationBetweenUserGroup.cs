namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRelationBetweenUserGroup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Group_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.Users", "Group_ID");
            AddForeignKey("dbo.Users", "Group_ID", "dbo.Groups", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "Group_ID", "dbo.Groups");
            DropIndex("dbo.Users", new[] { "Group_ID" });
            DropColumn("dbo.Users", "Group_ID");
        }
    }
}
