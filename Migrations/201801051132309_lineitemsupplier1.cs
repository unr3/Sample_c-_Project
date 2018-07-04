namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lineitemsupplier1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LineItemSuppliers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LineItemId = c.Int(nullable: false),
                        SupplierId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LineItems", t => t.LineItemId, cascadeDelete: true)
                .Index(t => t.LineItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LineItemSuppliers", "LineItemId", "dbo.LineItems");
            DropIndex("dbo.LineItemSuppliers", new[] { "LineItemId" });
            DropTable("dbo.LineItemSuppliers");
        }
    }
}
