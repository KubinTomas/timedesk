namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateUserWorkspace : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserWorkspaces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Invited = c.DateTime(nullable: false),
                        Joined = c.DateTime(nullable: false),
                        ApplicationUserId = c.Int(),
                        WorkspaceId = c.Int(),
                        StatusId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .ForeignKey("dbo.WorkSpaces", t => t.WorkspaceId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.WorkspaceId)
                .Index(t => t.StatusId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserWorkspaces", "WorkspaceId", "dbo.WorkSpaces");
            DropForeignKey("dbo.UserWorkspaces", "StatusId", "dbo.Status");
            DropForeignKey("dbo.UserWorkspaces", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserWorkspaces", new[] { "StatusId" });
            DropIndex("dbo.UserWorkspaces", new[] { "WorkspaceId" });
            DropIndex("dbo.UserWorkspaces", new[] { "ApplicationUserId" });
            DropTable("dbo.UserWorkspaces");
        }
    }
}
