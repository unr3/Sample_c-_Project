namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class calculatedprice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LineItems", "SelectedSupplierCalculatedPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LineItems", "SelectedSupplierCalculatedPrice");
        }
    }
}
