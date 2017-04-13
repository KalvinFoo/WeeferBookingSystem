namespace WeeferBookingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class x2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "CategoryId", c => c.Int(nullable: false));
            DropColumn("dbo.Bookings", "CategoryList");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bookings", "CategoryList", c => c.Guid(nullable: false));
            DropColumn("dbo.Bookings", "CategoryId");
        }
    }
}
