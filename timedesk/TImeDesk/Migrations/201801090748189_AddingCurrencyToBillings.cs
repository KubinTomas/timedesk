namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingCurrencyToBillings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Billings", "CurrencyId", c => c.Int(nullable: false));
            CreateIndex("dbo.Billings", "CurrencyId");
            AddForeignKey("dbo.Billings", "CurrencyId", "dbo.Currencies", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Billings", "CurrencyId", "dbo.Currencies");
            DropIndex("dbo.Billings", new[] { "CurrencyId" });
            DropColumn("dbo.Billings", "CurrencyId");
            DropTable("dbo.Currencies");
        }
    }
}
