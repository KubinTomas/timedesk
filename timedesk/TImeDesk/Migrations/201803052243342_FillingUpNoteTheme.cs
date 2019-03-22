namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FillingUpNoteTheme : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO NoteColorThemes (Header,Body) VALUES ('#2E880D','#83DA63')");
            Sql("INSERT INTO NoteColorThemes (Header,Body) VALUES ('#f8a713','#fbc259')");
            Sql("INSERT INTO NoteColorThemes (Header,Body) VALUES ('#2377e9','#79aff9')");

        }

        public override void Down()
        {
        }
    }
}
