namespace WeeferBookingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adding_Status_To_Booking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "Status");
        }
    }
}
