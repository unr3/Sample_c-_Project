namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SupplierPriceLineItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SupplierPriceLineItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileId = c.Int(nullable: false),
                        ReferanceId = c.Int(nullable: false),
                        LineItemId = c.Int(nullable: false),
                        SupplierPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Comment = c.String(),
                        Currency = c.String(),
                        Vat = c.Int(nullable: false),
                        IsClosed = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false,defaultValueSql:"GETDATE()"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LineItems", t => t.LineItemId, cascadeDelete: true)
                .Index(t => t.LineItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SupplierPriceLineItems", "LineItemId", "dbo.LineItems");
            DropIndex("dbo.SupplierPriceLineItems", new[] { "LineItemId" });
            DropTable("dbo.SupplierPriceLineItems");
        }
    }
}
