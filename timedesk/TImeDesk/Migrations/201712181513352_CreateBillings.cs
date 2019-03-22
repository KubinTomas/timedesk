namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateBillings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Billings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        HourRate = c.Int(nullable: false),
                        WorkSpaceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkSpaces", t => t.WorkSpaceId, cascadeDelete: true)
                .Index(t => t.WorkSpaceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Billings", "WorkSpaceId", "dbo.WorkSpaces");
            DropIndex("dbo.Billings", new[] { "WorkSpaceId" });
            DropTable("dbo.Billings");
        }
    }
}
