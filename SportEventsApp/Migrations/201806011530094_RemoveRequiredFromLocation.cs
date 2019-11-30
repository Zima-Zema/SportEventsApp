namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRequiredFromLocation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "Location_URL", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "Location_URL", c => c.String(nullable: false));
        }
    }
}
