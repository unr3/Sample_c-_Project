namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingSupplierNameToLineItemSupplier : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LineItemSuppliers", "SupplierName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LineItemSuppliers", "SupplierName");
        }
    }
}
