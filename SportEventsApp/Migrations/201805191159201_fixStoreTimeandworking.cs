namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixStoreTimeandworking : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Stores", "OpenTime", c => c.String(nullable: false));
            AlterColumn("dbo.Stores", "CloseTime", c => c.String(nullable: false));
            DropColumn("dbo.Stores", "WorkingHours");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stores", "WorkingHours", c => c.Int(nullable: false));
            AlterColumn("dbo.Stores", "CloseTime", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Stores", "OpenTime", c => c.Time(nullable: false, precision: 7));
        }
    }
}
