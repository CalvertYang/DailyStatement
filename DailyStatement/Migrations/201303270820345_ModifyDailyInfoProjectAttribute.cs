namespace DailyStatement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyDailyInfoProjectAttribute : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DailyInfoes", "Project_ProjectId", "dbo.Projects");
            DropIndex("dbo.DailyInfoes", new[] { "Project_ProjectId" });
            AlterColumn("dbo.DailyInfoes", "Project_ProjectId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DailyInfoes", "Project_ProjectId", c => c.Int());
        }
    }
}
