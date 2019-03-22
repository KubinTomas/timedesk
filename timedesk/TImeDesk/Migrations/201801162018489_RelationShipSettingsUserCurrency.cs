namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelationShipSettingsUserCurrency : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserSettings", "CurrencyId", c => c.Int(nullable: false));
            AddColumn("dbo.UserSettings", "ApplicationUserId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserSettings", "CurrencyId");
            CreateIndex("dbo.UserSettings", "ApplicationUserId");
            AddForeignKey("dbo.UserSettings", "ApplicationUserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserSettings", "CurrencyId", "dbo.Currencies", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserSettings", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.UserSettings", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserSettings", new[] { "ApplicationUserId" });
            DropIndex("dbo.UserSettings", new[] { "CurrencyId" });
            DropColumn("dbo.UserSettings", "ApplicationUserId");
            DropColumn("dbo.UserSettings", "CurrencyId");
        }
    }
}
