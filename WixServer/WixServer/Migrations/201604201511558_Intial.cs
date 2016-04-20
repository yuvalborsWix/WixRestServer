namespace WixServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Intial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GridItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GridId = c.Int(nullable: false),
                        ItemTypeId = c.Int(nullable: false),
                        xCoord = c.Int(nullable: false),
                        yCoord = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GridItemTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        xLength = c.Int(nullable: false),
                        yLength = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Grids",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RestaurantId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        GridType = c.Int(nullable: false),
                        Name = c.String(),
                        IsDefault = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RestaurantOwners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        RestaurantId = c.Int(nullable: false),
                        UserName = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GridId = c.Int(nullable: false),
                        TableNumber = c.Int(nullable: false),
                        Capacity = c.Int(nullable: false),
                        IsSmokingAllowed = c.Boolean(nullable: false),
                        xCoord = c.Int(nullable: false),
                        yCoord = c.Int(nullable: false),
                        xLength = c.Int(nullable: false),
                        yLength = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tables");
            DropTable("dbo.RestaurantOwners");
            DropTable("dbo.Grids");
            DropTable("dbo.GridItemTypes");
            DropTable("dbo.GridItems");
        }
    }
}
