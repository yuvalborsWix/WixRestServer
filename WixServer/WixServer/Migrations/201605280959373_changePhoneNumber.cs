namespace WixServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changePhoneNumber : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "PhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "PhoneNumber", c => c.Int(nullable: false));
        }
    }
}
