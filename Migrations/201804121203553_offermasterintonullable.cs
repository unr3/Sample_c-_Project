namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class offermasterintonullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SupplierPriceLineItems", "OfferMasterId", "dbo.OfferMasters");
            DropIndex("dbo.SupplierPriceLineItems", new[] { "OfferMasterId" });
            AlterColumn("dbo.SupplierPriceLineItems", "OfferMasterId", c => c.Int());
            CreateIndex("dbo.SupplierPriceLineItems", "OfferMasterId");
            AddForeignKey("dbo.SupplierPriceLineItems", "OfferMasterId", "dbo.OfferMasters", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SupplierPriceLineItems", "OfferMasterId", "dbo.OfferMasters");
            DropIndex("dbo.SupplierPriceLineItems", new[] { "OfferMasterId" });
            AlterColumn("dbo.SupplierPriceLineItems", "OfferMasterId", c => c.Int(nullable: false));
            CreateIndex("dbo.SupplierPriceLineItems", "OfferMasterId");
            AddForeignKey("dbo.SupplierPriceLineItems", "OfferMasterId", "dbo.OfferMasters", "Id", cascadeDelete: true);
        }
    }
}
