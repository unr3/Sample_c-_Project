namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderedlineitems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderLineItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReferanceNumberId = c.Int(nullable: false),
                        Impa = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Unit = c.String(nullable: false),
                        Qtty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Number = c.String(nullable: false),
                        Price = c.Decimal(precision: 18, scale: 2),
                        Remark = c.String(),
                        AltUnit = c.String(),
                        AltQtty = c.Decimal(precision: 18, scale: 2),
                        AltPrice = c.Decimal(precision: 18, scale: 2),
                        IsAlternative = c.Boolean(nullable: false),
                        MasterItemId = c.Int(),
                        UserId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        SelectedSupplierName = c.String(),
                        SelectedSupplierId = c.String(),
                        SelectedSupplierCalculatedPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SelectedSupplierPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SelectedSupplierRemark = c.String(),
                        SelectedSupplierCurrency = c.String(),
                        SelectedSupplier = c.Int(nullable: false),
                        IsRemoved = c.Boolean(nullable: false),
                        RemovedUser = c.String(),
                        MasterNumber = c.String(),
                        IsWareHouseSelected = c.Boolean(nullable: false),
                        WarehouseInfo = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ReferanceNumbers", t => t.ReferanceNumberId, cascadeDelete: true)
                .Index(t => t.ReferanceNumberId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderLineItems", "ReferanceNumberId", "dbo.ReferanceNumbers");
            DropIndex("dbo.OrderLineItems", new[] { "ReferanceNumberId" });
            DropTable("dbo.OrderLineItems");
        }
    }
}
