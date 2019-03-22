namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUserToProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "ApplicationUserId", c => c.Int());
            AddColumn("dbo.Projects", "ApplicationUserId", c => c.Int());
            CreateIndex("dbo.Clients", "ApplicationUserId");
            CreateIndex("dbo.Projects", "ApplicationUserId");
            AddForeignKey("dbo.Clients", "ApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Projects", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Clients", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "ApplicationUserId" });
            DropIndex("dbo.Clients", new[] { "ApplicationUserId" });
            DropColumn("dbo.Projects", "ApplicationUserId");
            DropColumn("dbo.Clients", "ApplicationUserId");
        }
    }
}
