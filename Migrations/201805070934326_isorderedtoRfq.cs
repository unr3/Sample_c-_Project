namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isorderedtoRfq : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReferanceNumbers", "IsOrdered", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReferanceNumbers", "IsOrdered");
        }
    }
}
