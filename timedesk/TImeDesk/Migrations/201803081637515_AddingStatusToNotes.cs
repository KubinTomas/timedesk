namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingStatusToNotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToDoNotes", "StatusId", c => c.Int());
            CreateIndex("dbo.ToDoNotes", "StatusId");
            AddForeignKey("dbo.ToDoNotes", "StatusId", "dbo.Status", "Id");


            Sql("INSERT INTO Status (Name) VALUES ('Private')");
            Sql("INSERT INTO Status (Name) VALUES ('Public')");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToDoNotes", "StatusId", "dbo.Status");
            DropIndex("dbo.ToDoNotes", new[] { "StatusId" });
            DropColumn("dbo.ToDoNotes", "StatusId");
        }
    }
}
