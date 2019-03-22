namespace TImeDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProject : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Projects", name: "WorkSpace_Id", newName: "WorkSpaceId");
            RenameIndex(table: "dbo.Projects", name: "IX_WorkSpace_Id", newName: "IX_WorkSpaceId");
            DropColumn("dbo.Projects", "WokSpaceId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "WokSpaceId", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.Projects", name: "IX_WorkSpaceId", newName: "IX_WorkSpace_Id");
            RenameColumn(table: "dbo.Projects", name: "WorkSpaceId", newName: "WorkSpace_Id");
        }
    }
}
