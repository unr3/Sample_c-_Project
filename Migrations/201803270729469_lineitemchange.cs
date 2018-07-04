namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lineitemchange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LineItems", "Number", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LineItems", "Number", c => c.Int(nullable: false));
        }
    }
}
