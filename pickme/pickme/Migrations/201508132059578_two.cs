namespace pickme.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class two : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Picks", name: "ApplicationUser_Id", newName: "PostedBy_Id");
            RenameIndex(table: "dbo.Picks", name: "IX_ApplicationUser_Id", newName: "IX_PostedBy_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Picks", name: "IX_PostedBy_Id", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Picks", name: "PostedBy_Id", newName: "ApplicationUser_Id");
        }
    }
}
