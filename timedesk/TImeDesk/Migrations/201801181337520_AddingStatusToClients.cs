namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingStatusToClients : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "StatusId", c => c.Int());
            CreateIndex("dbo.Clients", "StatusId");
            AddForeignKey("dbo.Clients", "StatusId", "dbo.Status", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clients", "StatusId", "dbo.Status");
            DropIndex("dbo.Clients", new[] { "StatusId" });
            DropColumn("dbo.Clients", "StatusId");
        }
    }
}
