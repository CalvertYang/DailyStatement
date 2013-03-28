namespace DailyStatement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RremoveRequiredToDailyInfoProjectNo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DailyInfoes", "ProjectNo", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DailyInfoes", "ProjectNo", c => c.String(nullable: false, maxLength: 200));
        }
    }
}
