namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class issplittedtorfq : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReferanceNumbers", "IsSplitted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReferanceNumbers", "IsSplitted");
        }
    }
}
