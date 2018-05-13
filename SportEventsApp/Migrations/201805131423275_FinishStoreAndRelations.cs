namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinishStoreAndRelations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoreName = c.String(),
                        Address = c.String(),
                        NumberOfDevices = c.Int(),
                        HoureFees = c.Double(),
                        WorkingHours = c.Int(nullable: false),
                        OpenTime = c.Time(nullable: false, precision: 7),
                        CloseTime = c.Time(nullable: false, precision: 7),
                        From = c.Int(nullable: false),
                        To = c.Int(nullable: false),
                        Approved = c.Boolean(),
                        CityId = c.String(nullable: false, maxLength: 128),
                        OwnerId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId)
                .Index(t => t.CityId)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.StorePhotos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        StoreId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stores", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId);
            
            AddColumn("dbo.Events", "Start", c => c.DateTime());
            AddColumn("dbo.Events", "End", c => c.DateTime());
            AddColumn("dbo.Events", "From", c => c.Time(precision: 7));
            AddColumn("dbo.Events", "To", c => c.Time(precision: 7));
            AddColumn("dbo.Events", "Published", c => c.Boolean());
            AddColumn("dbo.Events", "StoreId", c => c.Int());
            AddColumn("dbo.Matches", "CityId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Matches", "StoreId", c => c.Int());
            AddColumn("dbo.EntryFees", "Name", c => c.String());
            AlterColumn("dbo.Matches", "Type", c => c.Int());
            AlterColumn("dbo.Matches", "NofSlots", c => c.Int());
            AlterColumn("dbo.EntryFees", "Value", c => c.Int());
            CreateIndex("dbo.Matches", "CityId");
            CreateIndex("dbo.Matches", "StoreId");
            CreateIndex("dbo.Events", "StoreId");
            AddForeignKey("dbo.Matches", "CityId", "dbo.Cities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Events", "StoreId", "dbo.Stores", "Id");
            AddForeignKey("dbo.Matches", "StoreId", "dbo.Stores", "Id");
            DropColumn("dbo.Events", "Date");
            DropColumn("dbo.Events", "Time");
            DropColumn("dbo.Matches", "City");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Matches", "City", c => c.String());
            AddColumn("dbo.Events", "Time", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Events", "Date", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.StorePhotos", "StoreId", "dbo.Stores");
            DropForeignKey("dbo.Matches", "StoreId", "dbo.Stores");
            DropForeignKey("dbo.Events", "StoreId", "dbo.Stores");
            DropForeignKey("dbo.Stores", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Stores", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Matches", "CityId", "dbo.Cities");
            DropIndex("dbo.StorePhotos", new[] { "StoreId" });
            DropIndex("dbo.Events", new[] { "StoreId" });
            DropIndex("dbo.Stores", new[] { "Owner_Id" });
            DropIndex("dbo.Stores", new[] { "CityId" });
            DropIndex("dbo.Matches", new[] { "StoreId" });
            DropIndex("dbo.Matches", new[] { "CityId" });
            AlterColumn("dbo.EntryFees", "Value", c => c.Int(nullable: false));
            AlterColumn("dbo.Matches", "NofSlots", c => c.Int(nullable: false));
            AlterColumn("dbo.Matches", "Type", c => c.Int(nullable: false));
            DropColumn("dbo.EntryFees", "Name");
            DropColumn("dbo.Matches", "StoreId");
            DropColumn("dbo.Matches", "CityId");
            DropColumn("dbo.Events", "StoreId");
            DropColumn("dbo.Events", "Published");
            DropColumn("dbo.Events", "To");
            DropColumn("dbo.Events", "From");
            DropColumn("dbo.Events", "End");
            DropColumn("dbo.Events", "Start");
            DropTable("dbo.StorePhotos");
            DropTable("dbo.Stores");
        }
    }
}
