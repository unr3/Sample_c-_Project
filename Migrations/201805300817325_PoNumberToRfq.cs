namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PoNumberToRfq : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReferanceNumbers", "PONumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReferanceNumbers", "PONumber");
        }
    }
}
