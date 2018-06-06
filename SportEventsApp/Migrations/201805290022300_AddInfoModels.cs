namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInfoModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RegularInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        NestedId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NestedInfoes", t => t.NestedId)
                .Index(t => t.NestedId);
            
            CreateTable(
                "dbo.NestedInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Header = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InfoPoints",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false),
                        InfoId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RegularInfoes", t => t.InfoId)
                .Index(t => t.InfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InfoPoints", "InfoId", "dbo.RegularInfoes");
            DropForeignKey("dbo.RegularInfoes", "NestedId", "dbo.NestedInfoes");
            DropIndex("dbo.InfoPoints", new[] { "InfoId" });
            DropIndex("dbo.RegularInfoes", new[] { "NestedId" });
            DropTable("dbo.InfoPoints");
            DropTable("dbo.NestedInfoes");
            DropTable("dbo.RegularInfoes");
        }
    }
}
