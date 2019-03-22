namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingSymbolToCurrency : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Currencies", "Symbol", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Currencies", "Symbol");
        }
    }
}
