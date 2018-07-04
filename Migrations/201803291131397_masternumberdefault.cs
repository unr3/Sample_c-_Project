namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class masternumberdefault : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LineItems", "MasterNumber", c => c.String(nullable: false,defaultValueSql: "'0'"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LineItems", "MasterNumber", c => c.String());
        }
    }
}
