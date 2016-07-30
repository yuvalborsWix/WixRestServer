namespace WixServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_photo_name : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GridItems", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GridItems", "Name");
        }
    }
}
