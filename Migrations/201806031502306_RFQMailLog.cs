namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RFQMailLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RfqMailLogs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RfqId = c.Int(nullable: false),
                        ReceiverId = c.String(),
                        Date = c.DateTime(),
                        User = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RfqMailLogs");
        }
    }
}
