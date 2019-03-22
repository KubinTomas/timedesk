namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingNoteTheme : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NoteColorThemes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Header = c.String(),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ToDoNotes", "NoteColorThemeId", c => c.Int(nullable: false));
            CreateIndex("dbo.ToDoNotes", "NoteColorThemeId");
            AddForeignKey("dbo.ToDoNotes", "NoteColorThemeId", "dbo.NoteColorThemes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToDoNotes", "NoteColorThemeId", "dbo.NoteColorThemes");
            DropIndex("dbo.ToDoNotes", new[] { "NoteColorThemeId" });
            DropColumn("dbo.ToDoNotes", "NoteColorThemeId");
            DropTable("dbo.NoteColorThemes");
        }
    }
}
