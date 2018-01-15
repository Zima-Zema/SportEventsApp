namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCashNumberToUserModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "CashNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "CashNumber");
        }
    }
}
