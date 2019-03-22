namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingWorkSpaceIdToTasks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectTasks", "WorkSpaceId", c => c.Int());
            CreateIndex("dbo.ProjectTasks", "WorkSpaceId");
            AddForeignKey("dbo.ProjectTasks", "WorkSpaceId", "dbo.WorkSpaces", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectTasks", "WorkSpaceId", "dbo.WorkSpaces");
            DropIndex("dbo.ProjectTasks", new[] { "WorkSpaceId" });
            DropColumn("dbo.ProjectTasks", "WorkSpaceId");
        }
    }
}
