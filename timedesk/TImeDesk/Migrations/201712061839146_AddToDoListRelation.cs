namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddToDoListRelation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkSpaces", "ToDoListId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkSpaces", "ToDoListId");
        }
    }
}
