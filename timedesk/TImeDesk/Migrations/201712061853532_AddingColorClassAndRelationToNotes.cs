namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingColorClassAndRelationToNotes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Colors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ColorHex = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ToDoNotes", "ColorId", c => c.Int(nullable: false));
            CreateIndex("dbo.ToDoNotes", "ColorId");
            AddForeignKey("dbo.ToDoNotes", "ColorId", "dbo.Colors", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToDoNotes", "ColorId", "dbo.Colors");
            DropIndex("dbo.ToDoNotes", new[] { "ColorId" });
            DropColumn("dbo.ToDoNotes", "ColorId");
            DropTable("dbo.Colors");
        }
    }
}
