namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingDateAndFileNameToClient : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Clients", "FileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "FileName");
            DropColumn("dbo.Clients", "CreateDate");
        }
    }
}
