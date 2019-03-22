namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FillCurrenciesTable : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Currencies ( Name, Symbol) VALUES ('AUD','AUD')");
            Sql("INSERT INTO Currencies ( Name, Symbol) VALUES ('BRL','BRL')");
            Sql("INSERT INTO Currencies ( Name, Symbol) VALUES ('BGN','BGN')");
            Sql("INSERT INTO Currencies ( Name, Symbol) VALUES ('CNY','CNY')");
            Sql("INSERT INTO Currencies ( Name, Symbol) VALUES ('DKK','DKK')");
            Sql("INSERT INTO Currencies ( Name, Symbol) VALUES ('PHP','PHP')");
            Sql("INSERT INTO Currencies ( Name, Symbol) VALUES ('HKD','HKD')");
            Sql("INSERT INTO Currencies ( Name, Symbol) VALUES ('HRK','HRK')");
            Sql("INSERT INTO Currencies ( Name, Symbol) VALUES ('INR','INR')");
            Sql("INSERT INTO Currencies ( Name, Symbol) VALUES ('GBP','GBP')");        
        }
        
        public override void Down()
        {
        }
    }
}
