namespace WADAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class abc3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserCarts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserID = c.String(nullable: false),
                        ProductID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserCarts");
        }
    }
}
