namespace DailyStatement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompanyToEmployee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "Company", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "Company");
        }
    }
}
