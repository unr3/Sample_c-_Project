namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addoffermastertoSupplierPriceLine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SupplierPriceLineItems", "OfferMasterId", c => c.Int(nullable: false));
            CreateIndex("dbo.SupplierPriceLineItems", "OfferMasterId");
            AddForeignKey("dbo.SupplierPriceLineItems", "OfferMasterId", "dbo.OfferMasters", "Id", cascadeDelete: true);
            DropColumn("dbo.SupplierPriceLineItems", "OfferId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SupplierPriceLineItems", "OfferId", c => c.Int(nullable: false));
            DropForeignKey("dbo.SupplierPriceLineItems", "OfferMasterId", "dbo.OfferMasters");
            DropIndex("dbo.SupplierPriceLineItems", new[] { "OfferMasterId" });
            DropColumn("dbo.SupplierPriceLineItems", "OfferMasterId");
        }
    }
}
