namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lineitemidToOrderlineitem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderLineItems", "LineitemId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderLineItems", "LineitemId");
        }
    }
}
