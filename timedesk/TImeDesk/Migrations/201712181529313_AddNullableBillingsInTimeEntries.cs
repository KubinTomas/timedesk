namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNullableBillingsInTimeEntries : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TimeEntries", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.TimeEntries", "ProjectTaskId", "dbo.ProjectTasks");
            DropIndex("dbo.TimeEntries", new[] { "ProjectId" });
            DropIndex("dbo.TimeEntries", new[] { "ProjectTaskId" });
            AddColumn("dbo.TimeEntries", "BillingsId", c => c.Int());
            AlterColumn("dbo.TimeEntries", "ProjectId", c => c.Int());
            AlterColumn("dbo.TimeEntries", "ProjectTaskId", c => c.Int());
            CreateIndex("dbo.TimeEntries", "ProjectId");
            CreateIndex("dbo.TimeEntries", "ProjectTaskId");
            CreateIndex("dbo.TimeEntries", "BillingsId");
            AddForeignKey("dbo.TimeEntries", "BillingsId", "dbo.Billings", "Id");
            AddForeignKey("dbo.TimeEntries", "ProjectId", "dbo.Projects", "Id");
            AddForeignKey("dbo.TimeEntries", "ProjectTaskId", "dbo.ProjectTasks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeEntries", "ProjectTaskId", "dbo.ProjectTasks");
            DropForeignKey("dbo.TimeEntries", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.TimeEntries", "BillingsId", "dbo.Billings");
            DropIndex("dbo.TimeEntries", new[] { "BillingsId" });
            DropIndex("dbo.TimeEntries", new[] { "ProjectTaskId" });
            DropIndex("dbo.TimeEntries", new[] { "ProjectId" });
            AlterColumn("dbo.TimeEntries", "ProjectTaskId", c => c.Int(nullable: false));
            AlterColumn("dbo.TimeEntries", "ProjectId", c => c.Int(nullable: false));
            DropColumn("dbo.TimeEntries", "BillingsId");
            CreateIndex("dbo.TimeEntries", "ProjectTaskId");
            CreateIndex("dbo.TimeEntries", "ProjectId");
            AddForeignKey("dbo.TimeEntries", "ProjectTaskId", "dbo.ProjectTasks", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TimeEntries", "ProjectId", "dbo.Projects", "Id", cascadeDelete: true);
        }
    }
}
