
namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientAndProjectRelation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        WorkSpaceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkSpaces", t => t.WorkSpaceId, cascadeDelete: true)
                .Index(t => t.WorkSpaceId);
            
            AddColumn("dbo.Projects", "ClientId", c => c.Int(nullable: false));
            CreateIndex("dbo.Projects", "ClientId");
            AddForeignKey("dbo.Projects", "ClientId", "dbo.Clients", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Clients", "WorkSpaceId", "dbo.WorkSpaces");
            DropIndex("dbo.Clients", new[] { "WorkSpaceId" });
            DropIndex("dbo.Projects", new[] { "ClientId" });
            DropColumn("dbo.Projects", "ClientId");
            DropTable("dbo.Clients");
        }
    }
}
