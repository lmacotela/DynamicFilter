namespace DynamicFilter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "UserName", c => c.String());
            AlterColumn("dbo.Users", "Password", c => c.String());
            DropColumn("dbo.Users", "User_");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "User_", c => c.String());
            AlterColumn("dbo.Users", "Password", c => c.Int(nullable: false));
            DropColumn("dbo.Users", "UserName");
        }
    }
}
