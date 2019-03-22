namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FillStatusData : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Status (Name) VALUES ('Active')"); 
            Sql("INSERT INTO Status (Name) VALUES ('Archived')"); 
            Sql("INSERT INTO Status (Name) VALUES ('Deleted')"); 
          
        }
        
        public override void Down()
        {
        }
    }
}
