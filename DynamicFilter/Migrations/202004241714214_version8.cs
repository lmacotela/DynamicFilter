namespace DynamicFilter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version8 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Filters", "CreatedBy", "dbo.Users");
            DropIndex("dbo.Filters", new[] { "CreatedBy" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Filters", "CreatedBy");
            AddForeignKey("dbo.Filters", "CreatedBy", "dbo.Users", "UserID", cascadeDelete: true);
        }
    }
}
