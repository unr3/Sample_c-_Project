namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedfromsalereportToRfq : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReferanceNumbers", "RemovedFromSaleReport", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReferanceNumbers", "RemovedFromSaleReport");
        }
    }
}
