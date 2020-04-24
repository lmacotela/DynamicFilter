namespace DynamicFilter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version7 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Filters", "CreatedBy");
            AddForeignKey("dbo.Filters", "CreatedBy", "dbo.Users", "UserID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Filters", "CreatedBy", "dbo.Users");
            DropIndex("dbo.Filters", new[] { "CreatedBy" });
        }
    }
}
