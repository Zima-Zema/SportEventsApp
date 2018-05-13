namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeRelationBetweenUserStore : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Stores", "Owner_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Stores", new[] { "Owner_Id" });
            //DropColumn("dbo.Stores", "OwnerId");
            //DropColumn("dbo.Stores", "Owner_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stores", "Owner_Id", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Stores", "OwnerId", c => c.String());
            CreateIndex("dbo.Stores", "Owner_Id");
            AddForeignKey("dbo.Stores", "Owner_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
