namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingAutomaticTimerValue : DbMigration
    {
        public override void Up()
        {
          
            AddColumn("dbo.UserSettings", "AutomaticTimer", c => c.Boolean(nullable: false, defaultValue: true));
           
        }
        
        public override void Down()
        {
        }
    }
}
