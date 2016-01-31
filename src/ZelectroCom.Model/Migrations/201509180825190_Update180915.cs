namespace ZelectroCom.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update180915 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Article", "IndexHtml", c => c.String());
            AddColumn("dbo.Article", "ViewsCount", c => c.Int(nullable: false));
            AddColumn("dbo.Article", "Rating", c => c.Int(nullable: false));
            AddColumn("dbo.Section", "SectionState", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Section", "SectionState");
            DropColumn("dbo.Article", "Rating");
            DropColumn("dbo.Article", "ViewsCount");
            DropColumn("dbo.Article", "IndexHtml");
        }
    }
}
