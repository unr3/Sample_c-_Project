namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcurrencytoFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "Currency", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "Currency");
        }
    }
}
