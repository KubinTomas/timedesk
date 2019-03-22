namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingToDoListRelationAndClasses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ToDoLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ToDoNotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Header = c.String(),
                        ToDoListId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ToDoLists", t => t.ToDoListId, cascadeDelete: true)
                .Index(t => t.ToDoListId);
            
            CreateIndex("dbo.WorkSpaces", "ToDoListId");
            AddForeignKey("dbo.WorkSpaces", "ToDoListId", "dbo.ToDoLists", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkSpaces", "ToDoListId", "dbo.ToDoLists");
            DropForeignKey("dbo.ToDoNotes", "ToDoListId", "dbo.ToDoLists");
            DropIndex("dbo.WorkSpaces", new[] { "ToDoListId" });
            DropIndex("dbo.ToDoNotes", new[] { "ToDoListId" });
            DropTable("dbo.ToDoNotes");
            DropTable("dbo.ToDoLists");
        }
    }
}
