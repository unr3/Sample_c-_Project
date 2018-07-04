namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vadepesinAndDepartmentToFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "VadePesin", c => c.String());
            AddColumn("dbo.Files", "Department", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "Department");
            DropColumn("dbo.Files", "VadePesin");
        }
    }
}
