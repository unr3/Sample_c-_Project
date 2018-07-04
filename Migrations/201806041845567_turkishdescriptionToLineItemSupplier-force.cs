namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class turkishdescriptionToLineItemSupplierforce : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SupplierPriceLineItems", "TurkishDescription", c => c.String());
            DropColumn("dbo.LineItemSuppliers", "TurkishDescription");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LineItemSuppliers", "TurkishDescription", c => c.String());
            DropColumn("dbo.SupplierPriceLineItems", "TurkishDescription");
        }
    }
}
