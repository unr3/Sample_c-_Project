namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addReferanceNumber : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReferanceNumbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileId = c.Byte(nullable: false),
                        Name = c.String(),
                        CreatedDate = c.DateTime(),
                        DueDate = c.DateTime(),
                        Stage = c.Byte(nullable: false),
                        Isactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Files", t => t.FileId, cascadeDelete: true)
                .Index(t => t.FileId);
            
            AddColumn("dbo.Files", "Isactive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReferanceNumbers", "FileId", "dbo.Files");
            DropIndex("dbo.ReferanceNumbers", new[] { "FileId" });
            DropColumn("dbo.Files", "Isactive");
            DropTable("dbo.ReferanceNumbers");
        }
    }
}
