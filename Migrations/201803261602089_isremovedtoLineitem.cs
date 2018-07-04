namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isremovedtoLineitem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LineItems", "IsRemoved", c => c.Boolean(nullable: false));
            AddColumn("dbo.LineItems", "RemovedUser", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LineItems", "RemovedUser");
            DropColumn("dbo.LineItems", "IsRemoved");
        }
    }
}
