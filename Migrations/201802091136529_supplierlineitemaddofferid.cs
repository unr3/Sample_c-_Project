namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class supplierlineitemaddofferid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SupplierPriceLineItems", "OfferId", c => c.String());
            DropColumn("dbo.SupplierPriceLineItems", "FileId");
            DropColumn("dbo.SupplierPriceLineItems", "ReferanceId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SupplierPriceLineItems", "ReferanceId", c => c.Int(nullable: false));
            AddColumn("dbo.SupplierPriceLineItems", "FileId", c => c.Int(nullable: false));
            DropColumn("dbo.SupplierPriceLineItems", "OfferId");
        }
    }
}
