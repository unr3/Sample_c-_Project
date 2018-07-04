namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileMastercolumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "Eta", c => c.DateTime());
            AddColumn("dbo.Files", "Company", c => c.String());
            AddColumn("dbo.Files", "DeliveryPlace", c => c.String());
            AddColumn("dbo.Files", "DeliveryCost", c => c.Int(nullable: false));
            AddColumn("dbo.Files", "Discount", c => c.Int(nullable: false));
            AddColumn("dbo.Files", "Note1", c => c.String());
            AddColumn("dbo.Files", "Note2", c => c.String());
            AddColumn("dbo.Files", "Note3", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "Note3");
            DropColumn("dbo.Files", "Note2");
            DropColumn("dbo.Files", "Note1");
            DropColumn("dbo.Files", "Discount");
            DropColumn("dbo.Files", "DeliveryCost");
            DropColumn("dbo.Files", "DeliveryPlace");
            DropColumn("dbo.Files", "Company");
            DropColumn("dbo.Files", "Eta");
        }
    }
}
