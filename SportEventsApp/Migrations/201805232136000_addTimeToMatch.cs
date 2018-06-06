namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTimeToMatch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Matches", "Time", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Matches", "Time");
        }
    }
}
