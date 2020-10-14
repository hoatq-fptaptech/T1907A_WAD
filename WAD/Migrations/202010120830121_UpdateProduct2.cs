namespace WAD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProduct2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Qty", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Qty");
        }
    }
}
