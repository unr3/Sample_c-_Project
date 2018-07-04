namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LineItemNewColumnsForPricing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LineItems", "AlternativeQtty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LineItems", "AlternativePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LineItems", "AlternativeUnit", c => c.String());
            AddColumn("dbo.LineItems", "SelectedSupplierName", c => c.String());
            AddColumn("dbo.LineItems", "SelectedSupplierId", c => c.String());
            AddColumn("dbo.LineItems", "SelectedSupplierPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LineItems", "SelectedSupplierRemark", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LineItems", "SelectedSupplierRemark");
            DropColumn("dbo.LineItems", "SelectedSupplierPrice");
            DropColumn("dbo.LineItems", "SelectedSupplierId");
            DropColumn("dbo.LineItems", "SelectedSupplierName");
            DropColumn("dbo.LineItems", "AlternativeUnit");
            DropColumn("dbo.LineItems", "AlternativePrice");
            DropColumn("dbo.LineItems", "AlternativeQtty");
        }
    }
}
