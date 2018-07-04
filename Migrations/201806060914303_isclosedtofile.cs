namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isclosedtofile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "IsClosed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "IsClosed");
        }
    }
}
