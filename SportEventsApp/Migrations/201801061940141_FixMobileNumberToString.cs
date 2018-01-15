namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixMobileNumberToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EtisalatCashes", "Number", c => c.String());
            AlterColumn("dbo.Users", "Mobile", c => c.String());
            AlterColumn("dbo.VodafoneCashes", "Number", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VodafoneCashes", "Number", c => c.Long(nullable: false));
            AlterColumn("dbo.Users", "Mobile", c => c.Long(nullable: false));
            AlterColumn("dbo.EtisalatCashes", "Number", c => c.Long(nullable: false));
        }
    }
}
