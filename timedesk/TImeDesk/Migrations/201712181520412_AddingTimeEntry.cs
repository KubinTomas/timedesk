namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingTimeEntry : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TimeEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SpendedTime = c.Int(nullable: false),
                        StartedDate = c.DateTime(nullable: false),
                        EndedDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        IsFinished = c.Boolean(nullable: false),
                        ApplicationUserId = c.Int(nullable: false),
                        WorkspaceId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        ProjectTaskId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: false)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: false)
                .ForeignKey("dbo.ProjectTasks", t => t.ProjectTaskId, cascadeDelete: false)
                .ForeignKey("dbo.WorkSpaces", t => t.WorkspaceId, cascadeDelete: false)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.WorkspaceId)
                .Index(t => t.ProjectId)
                .Index(t => t.ProjectTaskId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeEntries", "WorkspaceId", "dbo.WorkSpaces");
            DropForeignKey("dbo.TimeEntries", "ProjectTaskId", "dbo.ProjectTasks");
            DropForeignKey("dbo.TimeEntries", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.TimeEntries", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.TimeEntries", new[] { "ProjectTaskId" });
            DropIndex("dbo.TimeEntries", new[] { "ProjectId" });
            DropIndex("dbo.TimeEntries", new[] { "WorkspaceId" });
            DropIndex("dbo.TimeEntries", new[] { "ApplicationUserId" });
            DropTable("dbo.TimeEntries");
        }
    }
}
