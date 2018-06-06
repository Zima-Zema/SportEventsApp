namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCashNumberToEventUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventUsers", "CashNumber", c => c.String());
            DropColumn("dbo.AspNetUsers", "CashNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "CashNumber", c => c.String());
            DropColumn("dbo.EventUsers", "CashNumber");
        }
    }
}
