namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserWorkspaceBillings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserWorkspaceBillings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.Int(nullable: false),
                        WorkspaceId = c.Int(),
                        BillingsId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .ForeignKey("dbo.Billings", t => t.BillingsId)
                .ForeignKey("dbo.WorkSpaces", t => t.WorkspaceId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.WorkspaceId)
                .Index(t => t.BillingsId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserWorkspaceBillings", "WorkspaceId", "dbo.WorkSpaces");
            DropForeignKey("dbo.UserWorkspaceBillings", "BillingsId", "dbo.Billings");
            DropForeignKey("dbo.UserWorkspaceBillings", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserWorkspaceBillings", new[] { "BillingsId" });
            DropIndex("dbo.UserWorkspaceBillings", new[] { "WorkspaceId" });
            DropIndex("dbo.UserWorkspaceBillings", new[] { "ApplicationUserId" });
            DropTable("dbo.UserWorkspaceBillings");
        }
    }
}
