namespace DinarakProject01.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialization_books1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookClasses", "ImageLink", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookClasses", "ImageLink");
        }
    }
}
