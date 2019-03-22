namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewDbSet : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.Int(nullable: false),
                        WorkSpaceId = c.Int(),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.WorkSpaces", t => t.WorkSpaceId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.WorkSpaceId)
                .Index(t => t.ProjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProjects", "WorkSpaceId", "dbo.WorkSpaces");
            DropForeignKey("dbo.UserProjects", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.UserProjects", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserProjects", new[] { "ProjectId" });
            DropIndex("dbo.UserProjects", new[] { "WorkSpaceId" });
            DropIndex("dbo.UserProjects", new[] { "ApplicationUserId" });
            DropTable("dbo.UserProjects");
        }
    }
}
