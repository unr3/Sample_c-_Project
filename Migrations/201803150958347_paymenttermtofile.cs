namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paymenttermtofile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "PaymentTerm", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "PaymentTerm");
        }
    }
}
