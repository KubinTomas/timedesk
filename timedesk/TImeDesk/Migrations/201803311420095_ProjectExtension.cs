namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectExtension : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "DeadLine", c => c.DateTime());
            AddColumn("dbo.Projects", "Budget", c => c.Single());
            AddColumn("dbo.Projects", "HourLimit", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "HourLimit");
            DropColumn("dbo.Projects", "Budget");
            DropColumn("dbo.Projects", "DeadLine");
        }
    }
}
