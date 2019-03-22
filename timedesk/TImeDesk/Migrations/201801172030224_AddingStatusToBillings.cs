namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingStatusToBillings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Billings", "StatusId", c => c.Int());
            CreateIndex("dbo.Billings", "StatusId");
            AddForeignKey("dbo.Billings", "StatusId", "dbo.Status", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Billings", "StatusId", "dbo.Status");
            DropIndex("dbo.Billings", new[] { "StatusId" });
            DropColumn("dbo.Billings", "StatusId");
        }
    }
}
