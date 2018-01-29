namespace EF6.Demo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestTimespanInMSSql : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "TimeStamp", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "TimeStamp");
        }
    }
}
