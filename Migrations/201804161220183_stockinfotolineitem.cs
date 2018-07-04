namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stockinfotolineitem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LineItems", "WarehouseInfo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LineItems", "WarehouseInfo");
        }
    }
}
