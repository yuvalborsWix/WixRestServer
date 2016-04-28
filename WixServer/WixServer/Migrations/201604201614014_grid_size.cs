namespace WixServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class grid_size : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Grids", "XLen", c => c.Int(nullable: false));
            AddColumn("dbo.Grids", "YLen", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Grids", "YLen");
            DropColumn("dbo.Grids", "XLen");
        }
    }
}
