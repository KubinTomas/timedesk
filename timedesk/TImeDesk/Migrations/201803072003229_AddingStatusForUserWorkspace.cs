namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingStatusForUserWorkspace : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Status (Name) VALUES ('Accepted')");
            Sql("INSERT INTO Status (Name) VALUES ('Pending')");
        }
        
        public override void Down()
        {
        }
    }
}
