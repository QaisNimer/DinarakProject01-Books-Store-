namespace DinarakProject01.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initilization1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DinarakClasses", "Role", c => c.String());
            DropColumn("dbo.DinarakClasses", "isAvailable");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DinarakClasses", "isAvailable", c => c.Boolean(nullable: false));
            DropColumn("dbo.DinarakClasses", "Role");
        }
    }
}
