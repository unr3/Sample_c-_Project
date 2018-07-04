namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lineitemnewcolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LineItems", "SelectedSupplierCurrency", c => c.String());
            AddColumn("dbo.LineItems", "SelectedSupplier", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LineItems", "SelectedSupplier");
            DropColumn("dbo.LineItems", "SelectedSupplierCurrency");
        }
    }
}
