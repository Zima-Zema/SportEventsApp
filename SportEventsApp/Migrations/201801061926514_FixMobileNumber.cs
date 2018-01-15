namespace SportEventsApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixMobileNumber : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EtisalatCashes", "Number", c => c.Long(nullable: false));
            AlterColumn("dbo.Users", "Mobile", c => c.Long(nullable: false));
            AlterColumn("dbo.VodafoneCashes", "Number", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VodafoneCashes", "Number", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "Mobile", c => c.Int(nullable: false));
            AlterColumn("dbo.EtisalatCashes", "Number", c => c.Int(nullable: false));
        }
    }
}
