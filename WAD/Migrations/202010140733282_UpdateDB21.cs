namespace WAD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDB21 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "BrandOfProduct_Id", "dbo.Brands");
            DropIndex("dbo.Products", new[] { "BrandOfProduct_Id" });
            RenameColumn(table: "dbo.Products", name: "BrandOfProduct_Id", newName: "BrandID");
            AlterColumn("dbo.Products", "BrandID", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "BrandID");
            AddForeignKey("dbo.Products", "BrandID", "dbo.Brands", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "BrandID", "dbo.Brands");
            DropIndex("dbo.Products", new[] { "BrandID" });
            AlterColumn("dbo.Products", "BrandID", c => c.Int());
            RenameColumn(table: "dbo.Products", name: "BrandID", newName: "BrandOfProduct_Id");
            CreateIndex("dbo.Products", "BrandOfProduct_Id");
            AddForeignKey("dbo.Products", "BrandOfProduct_Id", "dbo.Brands", "Id");
        }
    }
}
