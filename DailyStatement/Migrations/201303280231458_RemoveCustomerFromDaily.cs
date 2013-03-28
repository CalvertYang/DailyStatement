namespace DailyStatement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCustomerFromDaily : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DailyInfoes", "Customer", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DailyInfoes", "Customer", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
