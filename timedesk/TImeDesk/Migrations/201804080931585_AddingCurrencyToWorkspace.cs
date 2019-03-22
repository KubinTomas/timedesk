namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingCurrencyToWorkspace : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Currencies ( Name, Symbol) VALUES ('CZK','CZK')");
            AddColumn("dbo.WorkSpaces", "CurrencyId", c => c.Int());
            CreateIndex("dbo.WorkSpaces", "CurrencyId");
            AddForeignKey("dbo.WorkSpaces", "CurrencyId", "dbo.Currencies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkSpaces", "CurrencyId", "dbo.Currencies");
            DropIndex("dbo.WorkSpaces", new[] { "CurrencyId" });
            DropColumn("dbo.WorkSpaces", "CurrencyId");
        }
    }
}
