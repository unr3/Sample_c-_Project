namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedfromuseranddescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReferanceNumbers", "RemovedFromSaleReportUser", c => c.String());
            AddColumn("dbo.ReferanceNumbers", "RemovedFromSaleReportReason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReferanceNumbers", "RemovedFromSaleReportReason");
            DropColumn("dbo.ReferanceNumbers", "RemovedFromSaleReportUser");
        }
    }
}
