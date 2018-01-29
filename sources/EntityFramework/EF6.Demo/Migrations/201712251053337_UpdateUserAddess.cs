namespace EF6.Demo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserAddess : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.User", "Address", c => c.String());
            //DropColumn("dbo.User", "Address2");
            RenameColumn("dbo.User", "Address2","Address");
           
        }
        
        public override void Down()
        {
            //AddColumn("dbo.User", "Address2", c => c.String());
            //DropColumn("dbo.User", "Address");
            RenameColumn("dbo.User", "Address", "Address2");
        }
    }
}
