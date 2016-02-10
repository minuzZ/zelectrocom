namespace ZelectroCom.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update050216 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Firstname", c => c.String());
            AddColumn("dbo.AspNetUsers", "Lastname", c => c.String());
            AddColumn("dbo.AspNetUsers", "Description", c => c.String());
            AddColumn("dbo.AspNetUsers", "Rating", c => c.Double(false, 0));
            AddColumn("dbo.AspNetUsers", "HasAvatar", c => c.Boolean(false, false));
            AddColumn("dbo.Section", "Path", c => c.String());
            AddColumn("dbo.Section", "Description", c => c.String());
            AddColumn("dbo.Section", "SeoTitle", c => c.String());
            AddColumn("dbo.Section", "SeoDescription", c => c.String());
            AddColumn("dbo.Section", "SeoKeywords", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Section", "SeoKeywords");
            DropColumn("dbo.Section", "SeoDescription");
            DropColumn("dbo.Section", "SeoTitle");
            DropColumn("dbo.Section", "Description");
            DropColumn("dbo.Section", "Path");
            DropColumn("dbo.AspNetUsers", "HasAvatar");
            DropColumn("dbo.AspNetUsers", "Rating");
            DropColumn("dbo.AspNetUsers", "Description");
            DropColumn("dbo.AspNetUsers", "Lastname");
            DropColumn("dbo.AspNetUsers", "Firstname");
        }
    }
}
