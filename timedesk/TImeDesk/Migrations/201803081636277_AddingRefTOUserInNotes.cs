namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingRefTOUserInNotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToDoNotes", "ApplicationUserId", c => c.Int());
            CreateIndex("dbo.ToDoNotes", "ApplicationUserId");
            AddForeignKey("dbo.ToDoNotes", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToDoNotes", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.ToDoNotes", new[] { "ApplicationUserId" });
            DropColumn("dbo.ToDoNotes", "ApplicationUserId");
        }
    }
}
