namespace ZelectroCom.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update021015 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomUrl",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        ContentType = c.Int(nullable: false),
                        ContentId = c.Int(nullable: false),
                        ContentPath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OldMedia",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OldPath = c.String(),
                        NewPath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Article", "Hidden", c => c.Boolean(nullable: false));
            DropColumn("dbo.Section", "Path");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Section", "Path", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Article", "Hidden");
            DropTable("dbo.OldMedia");
            DropTable("dbo.CustomUrl");
        }
    }
}
