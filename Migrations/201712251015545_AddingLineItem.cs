namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingLineItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LineItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReferanceNumberId = c.Int(nullable: false),
                        Impa = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Unit = c.String(nullable: false),
                        Qtty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Number = c.Int(nullable: false),
                        Price = c.Decimal(precision: 18, scale: 2),
                        Remark = c.String(),
                        AltUnit = c.String(),
                        AltQtty = c.Decimal(precision: 18, scale: 2),
                        AltPrice = c.Decimal(precision: 18, scale: 2),
                        IsAlternative = c.Boolean(nullable: false),
                        MasterItemId = c.Int(),
                        UserId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ReferanceNumbers", t => t.ReferanceNumberId, cascadeDelete: true)
                .Index(t => t.ReferanceNumberId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LineItems", "ReferanceNumberId", "dbo.ReferanceNumbers");
            DropIndex("dbo.LineItems", new[] { "ReferanceNumberId" });
            DropTable("dbo.LineItems");
        }
    }
}
