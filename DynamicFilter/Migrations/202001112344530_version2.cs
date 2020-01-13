namespace DynamicFilter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Filters", "Description", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.Filters", "Detail", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Filters", "Detail", c => c.String());
            AlterColumn("dbo.Filters", "Description", c => c.String());
        }
    }
}
