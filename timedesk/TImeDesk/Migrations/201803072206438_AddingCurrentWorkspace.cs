namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingCurrentWorkspace : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserSettings", "currentWorkspace", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserSettings", "currentWorkspace");
        }
    }
}
