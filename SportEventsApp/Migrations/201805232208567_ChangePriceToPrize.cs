namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePriceToPrize : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Matches", "Prize", c => c.Double(nullable: false));
            DropColumn("dbo.Matches", "Price");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Matches", "Price", c => c.Double(nullable: false));
            DropColumn("dbo.Matches", "Prize");
        }
    }
}
