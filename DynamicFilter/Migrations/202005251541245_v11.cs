namespace DynamicFilter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ProveedorID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ProveedorID");
        }
    }
}
