namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingColorToTasks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectTasks", "ColorString", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectTasks", "ColorString");
        }
    }
}
