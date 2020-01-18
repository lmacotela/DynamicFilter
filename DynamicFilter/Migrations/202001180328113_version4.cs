namespace DynamicFilter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "RoleID", c => c.Int(nullable: false));
            CreateIndex("dbo.Users", "RoleID");
            AddForeignKey("dbo.Users", "RoleID", "dbo.Roles", "RoleID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RoleID", "dbo.Roles");
            DropIndex("dbo.Users", new[] { "RoleID" });
            DropColumn("dbo.Users", "RoleID");
        }
    }
}
