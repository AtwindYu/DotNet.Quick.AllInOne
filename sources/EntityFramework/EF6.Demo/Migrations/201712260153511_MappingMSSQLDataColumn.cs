namespace EF6.Demo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MappingMSSQLDataColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "BirthDay", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.User", "TimeStamp", c => c.Time(precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "TimeStamp", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.User", "BirthDay");
        }
    }
}
