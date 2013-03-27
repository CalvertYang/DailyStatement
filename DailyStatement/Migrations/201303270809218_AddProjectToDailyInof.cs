namespace DailyStatement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectToDailyInof : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DailyInfoes", "Project_ProjectId", c => c.Int());
            AddForeignKey("dbo.DailyInfoes", "Project_ProjectId", "dbo.Projects", "ProjectId");
            CreateIndex("dbo.DailyInfoes", "Project_ProjectId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.DailyInfoes", new[] { "Project_ProjectId" });
            DropForeignKey("dbo.DailyInfoes", "Project_ProjectId", "dbo.Projects");
            DropColumn("dbo.DailyInfoes", "Project_ProjectId");
        }
    }
}
