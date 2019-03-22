namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingHourRateToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Billings", "HourRate", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Billings", "HourRate", c => c.Int(nullable: false));
        }
    }
}
