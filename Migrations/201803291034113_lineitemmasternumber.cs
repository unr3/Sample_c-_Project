namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lineitemmasternumber : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LineItems", "MasterNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LineItems", "MasterNumber", c => c.Int());
        }
    }
}
