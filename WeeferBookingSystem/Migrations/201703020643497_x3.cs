namespace WeeferBookingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class x3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categories", "IsRequiredApproval", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Categories", "IsRequiredApproval", c => c.Boolean(nullable: false));
        }
    }
}
