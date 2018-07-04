namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class supplierpriceoldpricedate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SupplierPriceLineItems", "IsOldPrice", c => c.Boolean(nullable: false));
            AddColumn("dbo.SupplierPriceLineItems", "OldPriceDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SupplierPriceLineItems", "OldPriceDate");
            DropColumn("dbo.SupplierPriceLineItems", "IsOldPrice");
        }
    }
}
