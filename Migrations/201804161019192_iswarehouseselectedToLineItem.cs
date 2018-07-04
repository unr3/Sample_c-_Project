namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class iswarehouseselectedToLineItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LineItems", "IsWareHouseSelected", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LineItems", "IsWareHouseSelected");
        }
    }
}
