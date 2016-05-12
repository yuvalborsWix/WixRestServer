namespace WixServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cahnge : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GridId = c.Int(nullable: false),
                        TableNumber = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        NumOfPeople = c.Int(nullable: false),
                        FromTime = c.DateTime(nullable: false),
                        ToTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Restaurants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Grids", "XLen", c => c.Int(nullable: false));
            AddColumn("dbo.Grids", "YLen", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Grids", "YLen");
            DropColumn("dbo.Grids", "XLen");
            DropTable("dbo.Restaurants");
            DropTable("dbo.Orders");
        }
    }
}
