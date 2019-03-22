namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingBillingsToProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "BillingsId", c => c.Int());
            CreateIndex("dbo.Projects", "BillingsId");
            AddForeignKey("dbo.Projects", "BillingsId", "dbo.Billings", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "BillingsId", "dbo.Billings");
            DropIndex("dbo.Projects", new[] { "BillingsId" });
            DropColumn("dbo.Projects", "BillingsId");
        }
    }
}
