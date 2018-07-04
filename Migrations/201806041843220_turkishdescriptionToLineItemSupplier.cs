namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class turkishdescriptionToLineItemSupplier : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LineItemSuppliers", "TurkishDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LineItemSuppliers", "TurkishDescription");
        }
    }
}
