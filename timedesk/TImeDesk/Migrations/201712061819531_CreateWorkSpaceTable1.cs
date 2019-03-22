namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateWorkSpaceTable1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkSpaces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDefault = c.Boolean(nullable: false),
                        Created = c.DateTime(nullable: false),
                        ApplicationUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkSpaces", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.WorkSpaces", new[] { "ApplicationUserId" });
            DropTable("dbo.WorkSpaces");
        }
    }
}
