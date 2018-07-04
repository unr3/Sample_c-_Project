namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class offermastercomplateddate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OfferMasters", "ComplatedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OfferMasters", "ComplatedDate");
        }
    }
}
