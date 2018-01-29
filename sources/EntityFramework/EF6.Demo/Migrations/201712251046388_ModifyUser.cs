namespace EF6.Demo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Address2", c => c.String());
            DropColumn("dbo.User", "Address");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "Address", c => c.String());
            DropColumn("dbo.User", "Address2");
        }
    }
}
