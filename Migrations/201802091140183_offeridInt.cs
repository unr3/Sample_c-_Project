namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class offeridInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SupplierPriceLineItems", "OfferId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SupplierPriceLineItems", "OfferId", c => c.String());
        }
    }
}
