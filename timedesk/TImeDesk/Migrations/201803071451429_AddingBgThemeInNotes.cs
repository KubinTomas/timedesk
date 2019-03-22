namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingBgThemeInNotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NoteColorThemes", "ThemeBgColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NoteColorThemes", "ThemeBgColor");
        }
    }
}
