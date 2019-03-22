namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovingColorIdFromProject : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "ColorId", "dbo.Colors");
            DropIndex("dbo.Projects", new[] { "ColorId" });
            DropColumn("dbo.Projects", "ColorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "ColorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Projects", "ColorId");
            AddForeignKey("dbo.Projects", "ColorId", "dbo.Colors", "Id", cascadeDelete: true);
        }
    }
}
