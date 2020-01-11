namespace DynamicFilter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Enable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Filters",
                c => new
                    {
                        FilterID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Place = c.String(),
                        Detail = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        Enable = c.Boolean(nullable: false),
                        CategoryID = c.Int(nullable: false),
                        TypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FilterID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Types", t => t.TypeID, cascadeDelete: true)
                .Index(t => t.CategoryID)
                .Index(t => t.TypeID);
            
            CreateTable(
                "dbo.Types",
                c => new
                    {
                        TypeID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Enable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TypeID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        User_ = c.String(),
                        Password = c.Int(nullable: false),
                        Enable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Filters", "TypeID", "dbo.Types");
            DropForeignKey("dbo.Filters", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Filters", new[] { "TypeID" });
            DropIndex("dbo.Filters", new[] { "CategoryID" });
            DropTable("dbo.Users");
            DropTable("dbo.Types");
            DropTable("dbo.Filters");
            DropTable("dbo.Categories");
        }
    }
}
