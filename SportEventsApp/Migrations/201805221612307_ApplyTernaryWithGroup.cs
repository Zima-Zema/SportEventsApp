namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplyTernaryWithGroup : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UsersGroups", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UsersGroups", "GroupId", "dbo.Groups");
            DropIndex("dbo.UsersGroups", new[] { "UserId" });
            DropIndex("dbo.UsersGroups", new[] { "GroupId" });
            AddColumn("dbo.EventUsers", "GroupId", c => c.Int());
            CreateIndex("dbo.EventUsers", "GroupId");
            AddForeignKey("dbo.EventUsers", "GroupId", "dbo.Groups", "Id");
            DropTable("dbo.UsersGroups");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UsersGroups",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.GroupId });
            
            DropForeignKey("dbo.EventUsers", "GroupId", "dbo.Groups");
            DropIndex("dbo.EventUsers", new[] { "GroupId" });
            DropColumn("dbo.EventUsers", "GroupId");
            CreateIndex("dbo.UsersGroups", "GroupId");
            CreateIndex("dbo.UsersGroups", "UserId");
            AddForeignKey("dbo.UsersGroups", "GroupId", "dbo.Groups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UsersGroups", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
