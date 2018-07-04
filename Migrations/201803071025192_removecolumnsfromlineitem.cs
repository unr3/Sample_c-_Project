namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removecolumnsfromlineitem : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.LineItems", "AlternativeQtty");
            DropColumn("dbo.LineItems", "AlternativePrice");
            DropColumn("dbo.LineItems", "AlternativeUnit");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LineItems", "AlternativeUnit", c => c.String());
            AddColumn("dbo.LineItems", "AlternativePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LineItems", "AlternativeQtty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
