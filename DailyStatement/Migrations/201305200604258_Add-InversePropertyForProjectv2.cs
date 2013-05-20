namespace DailyStatement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInversePropertyForProjectv2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Projects", name: "Sale_EmployeeId", newName: "SaleId");
            RenameColumn(table: "dbo.Projects", name: "Engineer_EmployeeId", newName: "EngineerId");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Projects", name: "EngineerId", newName: "Engineer_EmployeeId");
            RenameColumn(table: "dbo.Projects", name: "SaleId", newName: "Sale_EmployeeId");
        }
    }
}
