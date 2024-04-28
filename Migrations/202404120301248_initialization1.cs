namespace DinarakProject01.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialization1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DinarakClasses", "isAvailable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DinarakClasses", "isAvailable");
        }
    }
}
