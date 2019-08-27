namespace WebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateschema2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "Type", c => c.String());
            AddColumn("dbo.SubAssets", "SubAssetUrl", c => c.String());
            DropColumn("dbo.SubAssets", "Type");
            DropColumn("dbo.SubAssets", "subDirectory");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SubAssets", "subDirectory", c => c.String());
            AddColumn("dbo.SubAssets", "Type", c => c.String());
            DropColumn("dbo.SubAssets", "SubAssetUrl");
            DropColumn("dbo.Assets", "Type");
        }
    }
}
