namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectStatusAndReferences : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        WokSpaceId = c.Int(nullable: false),
                        StatusId = c.Int(),
                        ColorId = c.Int(nullable: false),
                        WorkSpace_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Colors", t => t.ColorId, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .ForeignKey("dbo.WorkSpaces", t => t.WorkSpace_Id)
                .Index(t => t.StatusId)
                .Index(t => t.ColorId)
                .Index(t => t.WorkSpace_Id);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ProjectId = c.Int(nullable: false),
                        StatusId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.ProjectId)
                .Index(t => t.StatusId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectTasks", "StatusId", "dbo.Status");
            DropForeignKey("dbo.ProjectTasks", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Projects", "WorkSpace_Id", "dbo.WorkSpaces");
            DropForeignKey("dbo.Projects", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Projects", "ColorId", "dbo.Colors");
            DropIndex("dbo.ProjectTasks", new[] { "StatusId" });
            DropIndex("dbo.ProjectTasks", new[] { "ProjectId" });
            DropIndex("dbo.Projects", new[] { "WorkSpace_Id" });
            DropIndex("dbo.Projects", new[] { "ColorId" });
            DropIndex("dbo.Projects", new[] { "StatusId" });
            DropTable("dbo.ProjectTasks");
            DropTable("dbo.Status");
            DropTable("dbo.Projects");
        }
    }
}
