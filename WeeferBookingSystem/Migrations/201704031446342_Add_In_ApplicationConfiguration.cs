namespace WeeferBookingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_In_ApplicationConfiguration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationConfigurations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApprovalUsername = c.String(nullable: false),
                        SmtpUsername = c.String(),
                        SmtpPassword = c.String(),
                        SmtpFromName = c.String(),
                        SmtpHost = c.String(),
                        SmtpPort = c.Int(nullable: false),
                        SmtpEnableSsl = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ApplicationConfigurations");
        }
    }
}
