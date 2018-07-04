namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class impachangedtoLineItemSupplier : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LineItemSuppliers", "ImpaChanged", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LineItemSuppliers", "ImpaChanged");
        }
    }
}
