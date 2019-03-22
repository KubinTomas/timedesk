namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingTagAndTimeEntryTag : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        WorkSpaceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkSpaces", t => t.WorkSpaceId, cascadeDelete: true)
                .Index(t => t.WorkSpaceId);
            
            CreateTable(
                "dbo.TimeEntryTags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TagId = c.Int(nullable: false),
                        TimeEntryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tags", t => t.TagId, cascadeDelete: true)
                .ForeignKey("dbo.TimeEntries", t => t.TimeEntryId, cascadeDelete: true)
                .Index(t => t.TagId)
                .Index(t => t.TimeEntryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeEntryTags", "TimeEntryId", "dbo.TimeEntries");
            DropForeignKey("dbo.TimeEntryTags", "TagId", "dbo.Tags");
            DropForeignKey("dbo.Tags", "WorkSpaceId", "dbo.WorkSpaces");
            DropIndex("dbo.TimeEntryTags", new[] { "TimeEntryId" });
            DropIndex("dbo.TimeEntryTags", new[] { "TagId" });
            DropIndex("dbo.Tags", new[] { "WorkSpaceId" });
            DropTable("dbo.TimeEntryTags");
            DropTable("dbo.Tags");
        }
    }
}
