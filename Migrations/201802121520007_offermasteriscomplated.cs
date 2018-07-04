namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class offermasteriscomplated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OfferMasters", "IsComplated", c => c.Boolean(nullable: false));
            AddColumn("dbo.OfferMasters", "NameOfPriceGiven", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OfferMasters", "NameOfPriceGiven");
            DropColumn("dbo.OfferMasters", "IsComplated");
        }
    }
}
