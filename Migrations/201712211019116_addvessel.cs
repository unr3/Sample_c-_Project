namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addvessel : DbMigration
    {
        public override void Up()
        {
          
            
            AddColumn("dbo.Files", "VesselId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "VesselId");
           
        }
    }
}
