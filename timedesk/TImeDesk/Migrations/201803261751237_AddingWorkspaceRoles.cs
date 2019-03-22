namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingWorkspaceRoles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserWorkspaceRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CanManageRoles = c.Boolean(nullable: false),
                        CanManageProjects = c.Boolean(nullable: false),
                        CanManageBudget = c.Boolean(nullable: false),
                        CanManageStatistic = c.Boolean(nullable: false),
                        CanManageSettings = c.Boolean(nullable: false),
                        CanInviteUser = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.UserWorkspaces", "UserWorkspaceRolesId", c => c.Int());
            CreateIndex("dbo.UserWorkspaces", "UserWorkspaceRolesId");
            AddForeignKey("dbo.UserWorkspaces", "UserWorkspaceRolesId", "dbo.UserWorkspaceRoles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserWorkspaces", "UserWorkspaceRolesId", "dbo.UserWorkspaceRoles");
            DropIndex("dbo.UserWorkspaces", new[] { "UserWorkspaceRolesId" });
            DropColumn("dbo.UserWorkspaces", "UserWorkspaceRolesId");
            DropTable("dbo.UserWorkspaceRoles");
        }
    }
}
