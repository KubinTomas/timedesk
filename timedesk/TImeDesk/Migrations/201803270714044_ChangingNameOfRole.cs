namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingNameOfRole : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserWorkspaceRoles", "CanManageUsers", c => c.Boolean(nullable: false));
            DropColumn("dbo.UserWorkspaceRoles", "CanInviteUser");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserWorkspaceRoles", "CanInviteUser", c => c.Boolean(nullable: false));
            DropColumn("dbo.UserWorkspaceRoles", "CanManageUsers");
        }
    }
}
