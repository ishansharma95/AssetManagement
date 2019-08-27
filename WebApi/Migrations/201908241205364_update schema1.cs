namespace WebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateschema1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Assets", "UserId");
            AddForeignKey("dbo.Assets", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assets", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Assets", new[] { "UserId" });
            DropColumn("dbo.Assets", "UserId");
        }
    }
}
