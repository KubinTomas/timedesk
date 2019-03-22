namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingStatusToWorkspaceBilling : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserWorkspaceBillings", "StatusId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserWorkspaceBillings", "StatusId");
            AddForeignKey("dbo.UserWorkspaceBillings", "StatusId", "dbo.Status", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserWorkspaceBillings", "StatusId", "dbo.Status");
            DropIndex("dbo.UserWorkspaceBillings", new[] { "StatusId" });
            DropColumn("dbo.UserWorkspaceBillings", "StatusId");
        }
    }
}
