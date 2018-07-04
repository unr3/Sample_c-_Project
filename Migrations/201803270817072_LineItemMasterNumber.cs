namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LineItemMasterNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LineItems", "MasterNumber", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LineItems", "MasterNumber");
        }
    }
}
