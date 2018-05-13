namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMatchAndEntryFeesModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Matches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        City = c.String(),
                        Date = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        NofSlots = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        EntryFeesId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EntryFees", t => t.EntryFeesId, cascadeDelete: true)
                .Index(t => t.EntryFeesId);
            
            CreateTable(
                "dbo.EntryFees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UsersMatches",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        MatchId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.MatchId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Matches", t => t.MatchId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.MatchId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsersMatches", "MatchId", "dbo.Matches");
            DropForeignKey("dbo.UsersMatches", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Matches", "EntryFeesId", "dbo.EntryFees");
            DropIndex("dbo.UsersMatches", new[] { "MatchId" });
            DropIndex("dbo.UsersMatches", new[] { "UserId" });
            DropIndex("dbo.Matches", new[] { "EntryFeesId" });
            DropTable("dbo.UsersMatches");
            DropTable("dbo.EntryFees");
            DropTable("dbo.Matches");
        }
    }
}
