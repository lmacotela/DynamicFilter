namespace DynamicFilter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v13_MoreFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Types", "NameEN", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Types", "NameEN");
        }
    }
}
