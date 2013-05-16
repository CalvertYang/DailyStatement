namespace DailyStatement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyDatatypeOfProject : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "Price", c => c.Int());
            AlterColumn("dbo.Projects", "DownPayment", c => c.Int());
            AlterColumn("dbo.Projects", "HandoverPrice", c => c.Int());
            AlterColumn("dbo.Projects", "ConstitutePrice", c => c.Int());
            AlterColumn("dbo.Projects", "CheckPrice", c => c.Int());
            AlterColumn("dbo.Projects", "WarrantyPrice", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "WarrantyPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Projects", "CheckPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Projects", "ConstitutePrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Projects", "HandoverPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Projects", "DownPayment", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Projects", "Price", c => c.Int(nullable: false));
        }
    }
}
