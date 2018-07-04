namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Supplierscity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SuppliersCities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SupplierId = c.String(),
                        SupplierCity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SuppliersCities");
        }
    }
}
