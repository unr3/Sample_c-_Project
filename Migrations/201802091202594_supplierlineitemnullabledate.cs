namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class supplierlineitemnullabledate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SupplierPriceLineItems", "CreatedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SupplierPriceLineItems", "CreatedDate", c => c.DateTime(nullable: false));
        }
    }
}
