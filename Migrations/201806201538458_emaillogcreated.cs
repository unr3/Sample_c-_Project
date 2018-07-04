namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class emaillogcreated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmailLogs", "CreatedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmailLogs", "CreatedDate");
        }
    }
}
