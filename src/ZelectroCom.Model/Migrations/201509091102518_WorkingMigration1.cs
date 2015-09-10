namespace ZelectroCom.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkingMigration1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Article", "ViewsCount", c => c.Int(nullable: false));
            AddColumn("dbo.Section", "SectionState", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Section", "SectionState");
            DropColumn("dbo.Article", "ViewsCount");
        }
    }
}
