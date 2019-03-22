namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveColorRefFromNotes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ToDoNotes", "ColorId", "dbo.Colors");
            DropIndex("dbo.ToDoNotes", new[] { "ColorId" });
            DropColumn("dbo.ToDoNotes", "ColorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ToDoNotes", "ColorId", c => c.Int(nullable: false));
            CreateIndex("dbo.ToDoNotes", "ColorId");
            AddForeignKey("dbo.ToDoNotes", "ColorId", "dbo.Colors", "Id", cascadeDelete: true);
        }
    }
}
