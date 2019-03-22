namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingColorStringToProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "ColorString", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "ColorString");
        }
    }
}
