namespace DailyStatement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetDbSet : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectId = c.Int(nullable: false, identity: true),
                        ProjectNo = c.String(),
                        Comment = c.String(),
                        StartOn = c.DateTime(),
                        EndOn = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ProjectId);
            
            CreateTable(
                "dbo.Predictions",
                c => new
                    {
                        PredictionId = c.Int(nullable: false, identity: true),
                        PredictHours = c.Int(nullable: false),
                        WorkCategory_WorkCategoryId = c.Int(),
                        Project_ProjectId = c.Int(),
                    })
                .PrimaryKey(t => t.PredictionId)
                .ForeignKey("dbo.WorkCategories", t => t.WorkCategory_WorkCategoryId)
                .ForeignKey("dbo.Projects", t => t.Project_ProjectId)
                .Index(t => t.WorkCategory_WorkCategoryId)
                .Index(t => t.Project_ProjectId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Predictions", new[] { "Project_ProjectId" });
            DropIndex("dbo.Predictions", new[] { "WorkCategory_WorkCategoryId" });
            DropForeignKey("dbo.Predictions", "Project_ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Predictions", "WorkCategory_WorkCategoryId", "dbo.WorkCategories");
            DropTable("dbo.Predictions");
            DropTable("dbo.Projects");
        }
    }
}
