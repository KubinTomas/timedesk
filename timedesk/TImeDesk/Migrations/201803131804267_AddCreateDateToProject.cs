namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreateDateToProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Created", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "Created");
        }
    }
}
