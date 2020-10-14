namespace WAD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDB : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductOrders", "ProductID", "dbo.Products");
            DropIndex("dbo.ProductOrders", new[] { "ProductID" });
            RenameColumn(table: "dbo.ProductOrders", name: "ProductID", newName: "Product_Id");
            AlterColumn("dbo.ProductOrders", "Product_Id", c => c.Int());
            CreateIndex("dbo.ProductOrders", "Product_Id");
            AddForeignKey("dbo.ProductOrders", "Product_Id", "dbo.Products", "Id");
            DropColumn("dbo.ProductOrders", "OderID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductOrders", "OderID", c => c.Int(nullable: false));
            DropForeignKey("dbo.ProductOrders", "Product_Id", "dbo.Products");
            DropIndex("dbo.ProductOrders", new[] { "Product_Id" });
            AlterColumn("dbo.ProductOrders", "Product_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.ProductOrders", name: "Product_Id", newName: "ProductID");
            CreateIndex("dbo.ProductOrders", "ProductID");
            AddForeignKey("dbo.ProductOrders", "ProductID", "dbo.Products", "Id", cascadeDelete: true);
        }
    }
}
