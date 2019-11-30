namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManyToManyBetweenUsersAndGroups : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Group_ID", "dbo.Groups");
            DropIndex("dbo.AspNetUsers", new[] { "Group_ID" });
            CreateTable(
                "dbo.UsersGroups",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.GroupId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.GroupId);
            
            DropColumn("dbo.AspNetUsers", "Group_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Group_ID", c => c.Int());
            DropForeignKey("dbo.UsersGroups", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.UsersGroups", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UsersGroups", new[] { "GroupId" });
            DropIndex("dbo.UsersGroups", new[] { "UserId" });
            DropTable("dbo.UsersGroups");
            CreateIndex("dbo.AspNetUsers", "Group_ID");
            AddForeignKey("dbo.AspNetUsers", "Group_ID", "dbo.Groups", "Id");
        }
    }
}
