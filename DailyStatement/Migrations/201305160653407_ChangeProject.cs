namespace DailyStatement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "DownPayment", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Projects", "OrderOn", c => c.DateTime());
            AddColumn("dbo.Projects", "GetDownPaymentOn", c => c.DateTime());
            AddColumn("dbo.Projects", "HandoverPrice", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Projects", "HandoverPriceExpectOn", c => c.DateTime());
            AddColumn("dbo.Projects", "ConstitutePrice", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Projects", "ConstitutePriceExpectOn", c => c.DateTime());
            AddColumn("dbo.Projects", "CheckPrice", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Projects", "CheckPriceExpectOn", c => c.DateTime());
            AddColumn("dbo.Projects", "WarrantyPrice", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Projects", "WarrantyPriceExpectOn", c => c.DateTime());
            AddColumn("dbo.Projects", "ClosedOn", c => c.DateTime());
            AddColumn("dbo.Projects", "HandoverExpectOn", c => c.DateTime());
            AddColumn("dbo.Projects", "HandoverOn", c => c.DateTime());
            AddColumn("dbo.Projects", "ConstituteExpectOn", c => c.DateTime());
            AddColumn("dbo.Projects", "ConstituteOn", c => c.DateTime());
            AddColumn("dbo.Projects", "CheckExpectOn", c => c.DateTime());
            AddColumn("dbo.Projects", "CheckOn", c => c.DateTime());
            AddColumn("dbo.Projects", "HandoverPriceOn", c => c.DateTime());
            AddColumn("dbo.Projects", "ConstitutePriceOn", c => c.DateTime());
            AddColumn("dbo.Projects", "CheckPriceOn", c => c.DateTime());
            AddColumn("dbo.Projects", "WarrantyPriceOn", c => c.DateTime());
            AddColumn("dbo.Projects", "Sale_EmployeeId", c => c.Int());
            AddColumn("dbo.Projects", "Engineer_EmployeeId", c => c.Int());
            AddForeignKey("dbo.Projects", "Sale_EmployeeId", "dbo.Employees", "EmployeeId");
            AddForeignKey("dbo.Projects", "Engineer_EmployeeId", "dbo.Employees", "EmployeeId");
            CreateIndex("dbo.Projects", "Sale_EmployeeId");
            CreateIndex("dbo.Projects", "Engineer_EmployeeId");
            DropColumn("dbo.Projects", "IsClosed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "IsClosed", c => c.Boolean(nullable: false));
            DropIndex("dbo.Projects", new[] { "Engineer_EmployeeId" });
            DropIndex("dbo.Projects", new[] { "Sale_EmployeeId" });
            DropForeignKey("dbo.Projects", "Engineer_EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Projects", "Sale_EmployeeId", "dbo.Employees");
            DropColumn("dbo.Projects", "Engineer_EmployeeId");
            DropColumn("dbo.Projects", "Sale_EmployeeId");
            DropColumn("dbo.Projects", "WarrantyPriceOn");
            DropColumn("dbo.Projects", "CheckPriceOn");
            DropColumn("dbo.Projects", "ConstitutePriceOn");
            DropColumn("dbo.Projects", "HandoverPriceOn");
            DropColumn("dbo.Projects", "CheckOn");
            DropColumn("dbo.Projects", "CheckExpectOn");
            DropColumn("dbo.Projects", "ConstituteOn");
            DropColumn("dbo.Projects", "ConstituteExpectOn");
            DropColumn("dbo.Projects", "HandoverOn");
            DropColumn("dbo.Projects", "HandoverExpectOn");
            DropColumn("dbo.Projects", "ClosedOn");
            DropColumn("dbo.Projects", "WarrantyPriceExpectOn");
            DropColumn("dbo.Projects", "WarrantyPrice");
            DropColumn("dbo.Projects", "CheckPriceExpectOn");
            DropColumn("dbo.Projects", "CheckPrice");
            DropColumn("dbo.Projects", "ConstitutePriceExpectOn");
            DropColumn("dbo.Projects", "ConstitutePrice");
            DropColumn("dbo.Projects", "HandoverPriceExpectOn");
            DropColumn("dbo.Projects", "HandoverPrice");
            DropColumn("dbo.Projects", "GetDownPaymentOn");
            DropColumn("dbo.Projects", "OrderOn");
            DropColumn("dbo.Projects", "DownPayment");
        }
    }
}
