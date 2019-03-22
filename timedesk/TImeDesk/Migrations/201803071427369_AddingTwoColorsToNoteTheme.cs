namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingTwoColorsToNoteTheme : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO NoteColorThemes (Header,Body) VALUES ('#9500af','#e6a9f1')");
            Sql("INSERT INTO NoteColorThemes (Header,Body) VALUES ('#7e7e7e','#dddddd')");
        }
        
        public override void Down()
        {
        }
    }
}
