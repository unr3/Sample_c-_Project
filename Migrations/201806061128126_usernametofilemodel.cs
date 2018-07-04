namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usernametofilemodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "Createduser", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "Createduser");
        }
    }
}
