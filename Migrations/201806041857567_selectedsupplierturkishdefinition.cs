namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class selectedsupplierturkishdefinition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LineItems", "SelectedSupplierTurkishDefinition", c => c.String());
            AddColumn("dbo.OrderLineItems", "SelectedSupplierTurkishDefinition", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderLineItems", "SelectedSupplierTurkishDefinition");
            DropColumn("dbo.LineItems", "SelectedSupplierTurkishDefinition");
        }
    }
}
