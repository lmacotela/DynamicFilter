namespace DynamicFilter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.States",
                c => new
                    {
                        StateID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Enable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StateID);
            
            AddColumn("dbo.Filters", "StateID", c => c.Int(nullable: false));
            CreateIndex("dbo.Filters", "StateID");
            AddForeignKey("dbo.Filters", "StateID", "dbo.States", "StateID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Filters", "StateID", "dbo.States");
            DropIndex("dbo.Filters", new[] { "StateID" });
            DropColumn("dbo.Filters", "StateID");
            DropTable("dbo.States");
        }
    }
}
