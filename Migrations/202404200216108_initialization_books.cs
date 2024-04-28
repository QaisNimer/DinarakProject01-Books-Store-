namespace DinarakProject01.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialization_books : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookClasses",
                c => new
                    {
                        SerialNumber = c.Int(nullable: false, identity: true),
                        NumOfBooks = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        BookName = c.String(nullable: false),
                        Author = c.String(),
                        TypeOfBook = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.SerialNumber);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BookClasses");
        }
    }
}
