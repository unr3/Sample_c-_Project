namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mailsentandrevisionToOrderedLineItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderLineItems", "MailSent", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderLineItems", "OnRevision", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderLineItems", "OnRevision");
            DropColumn("dbo.OrderLineItems", "MailSent");
        }
    }
}
