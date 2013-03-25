namespace DailyStatement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "IsClosed", c => c.Boolean(nullable: false));
            DropColumn("dbo.Projects", "EndOn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "EndOn", c => c.DateTime());
            DropColumn("dbo.Projects", "IsClosed");
        }
    }
}
