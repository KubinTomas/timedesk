namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingMoreStatus : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Status (Name) VALUES ('LeftWorkspace')");
        }
        
        public override void Down()
        {
        }
    }
}
