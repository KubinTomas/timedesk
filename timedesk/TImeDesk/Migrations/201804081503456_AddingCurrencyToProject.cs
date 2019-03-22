namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingCurrencyToProject : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "BillingsId", "dbo.Billings");
            DropIndex("dbo.Projects", new[] { "BillingsId" });
            AddColumn("dbo.Projects", "CurrencyId", c => c.Int());
            CreateIndex("dbo.Projects", "CurrencyId");
            AddForeignKey("dbo.Projects", "CurrencyId", "dbo.Currencies", "Id");
            DropColumn("dbo.Projects", "BillingsId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "BillingsId", c => c.Int());
            DropForeignKey("dbo.Projects", "CurrencyId", "dbo.Currencies");
            DropIndex("dbo.Projects", new[] { "CurrencyId" });
            DropColumn("dbo.Projects", "CurrencyId");
            CreateIndex("dbo.Projects", "BillingsId");
            AddForeignKey("dbo.Projects", "BillingsId", "dbo.Billings", "Id");
        }
    }
}
