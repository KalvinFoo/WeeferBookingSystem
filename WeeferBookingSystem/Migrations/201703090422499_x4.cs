namespace WeeferBookingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class x4 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Bookings", "CategoryId");
            AddForeignKey("dbo.Bookings", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Bookings", new[] { "CategoryId" });
        }
    }
}
