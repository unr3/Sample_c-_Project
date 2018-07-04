namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcurrencytoFileupdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Files", "Currency", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Files", "Currency", c => c.Int(nullable: false));
        }
    }
}
