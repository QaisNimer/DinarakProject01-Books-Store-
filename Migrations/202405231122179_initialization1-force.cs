namespace DinarakProject01.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialization1force : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        SerialNumber = c.Int(nullable: false),
                        GuestId = c.Guid(nullable: false),
                        CommentDescription = c.String(),
                        Rating = c.Int(nullable: false),
                        CommentedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.BookClasses", t => t.SerialNumber)
                .Index(t => t.SerialNumber);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "SerialNumber", "dbo.BookClasses");
            DropIndex("dbo.Comments", new[] { "SerialNumber" });
            DropTable("dbo.Comments");
        }
    }
}
