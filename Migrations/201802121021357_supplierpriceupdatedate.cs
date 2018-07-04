namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class supplierpriceupdatedate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SupplierPriceLineItems", "UpdateDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SupplierPriceLineItems", "UpdateDate");
        }
    }
}
