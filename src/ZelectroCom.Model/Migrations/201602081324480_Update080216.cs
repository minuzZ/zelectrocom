namespace ZelectroCom.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update080216 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ZDev",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ImagePath = c.String(),
                        Description = c.String(),
                        Url = c.String(),
                        Order = c.Int(nullable: false),
                        ZDevState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Article", "OrderInSection", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Article", "OrderInSection");
            DropTable("dbo.ZDev");
        }
    }
}
