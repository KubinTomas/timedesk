namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingUserBillings : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserWorkspaceBillings", "StatusId", "dbo.Status");
            DropIndex("dbo.UserWorkspaceBillings", new[] { "WorkspaceId" });
            DropIndex("dbo.UserWorkspaceBillings", new[] { "StatusId" });
            AlterColumn("dbo.UserWorkspaceBillings", "StatusId", c => c.Int());
            CreateIndex("dbo.UserWorkspaceBillings", "WorkSpaceId");
            CreateIndex("dbo.UserWorkspaceBillings", "StatusId");
            AddForeignKey("dbo.UserWorkspaceBillings", "StatusId", "dbo.Status", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserWorkspaceBillings", "StatusId", "dbo.Status");
            DropIndex("dbo.UserWorkspaceBillings", new[] { "StatusId" });
            DropIndex("dbo.UserWorkspaceBillings", new[] { "WorkSpaceId" });
            AlterColumn("dbo.UserWorkspaceBillings", "StatusId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserWorkspaceBillings", "StatusId");
            CreateIndex("dbo.UserWorkspaceBillings", "WorkspaceId");
            AddForeignKey("dbo.UserWorkspaceBillings", "StatusId", "dbo.Status", "Id", cascadeDelete: true);
        }
    }
}
