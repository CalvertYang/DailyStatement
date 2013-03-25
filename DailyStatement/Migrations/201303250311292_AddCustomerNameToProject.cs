namespace DailyStatement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerNameToProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "CustomerName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "CustomerName");
        }
    }
}
