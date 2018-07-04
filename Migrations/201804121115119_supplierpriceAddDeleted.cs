namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class supplierpriceAddDeleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SupplierPriceLineItems", "Deleted", c => c.Boolean(nullable: false));
           
        }
        
        public override void Down()
        {
            
            DropColumn("dbo.SupplierPriceLineItems", "Deleted");
        }
    }
}
