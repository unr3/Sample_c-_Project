namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class offermasters : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OfferMasters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OfferId = c.String(),
                        SupplierId = c.String(),
                        FileId = c.String(),
                        IsClosed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OfferMasters");
        }
    }
}
