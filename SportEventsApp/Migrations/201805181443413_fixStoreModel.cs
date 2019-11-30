namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixStoreModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Stores", "StoreName", c => c.String(nullable: false));
            AlterColumn("dbo.Stores", "Address", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stores", "Address", c => c.String());
            AlterColumn("dbo.Stores", "StoreName", c => c.String());
        }
    }
}
