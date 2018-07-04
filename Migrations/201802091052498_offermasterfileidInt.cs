namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class offermasterfileidInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OfferMasters", "FileId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OfferMasters", "FileId", c => c.String());
        }
    }
}
