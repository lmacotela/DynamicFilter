namespace DynamicFilter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v12_MoreFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ProviderName", c => c.String());
            AddColumn("dbo.Users", "ContactPerson", c => c.String());
            AddColumn("dbo.Users", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Email");
            DropColumn("dbo.Users", "ContactPerson");
            DropColumn("dbo.Users", "ProviderName");
        }
    }
}
