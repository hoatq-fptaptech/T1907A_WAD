namespace WAD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrderProduct : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductOrders", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.ProductOrders", "Product_Id", "dbo.Products");
            DropIndex("dbo.ProductOrders", new[] { "Order_Id" });
            DropIndex("dbo.ProductOrders", new[] { "Product_Id" });
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BrandName = c.String(),
                        BrandImage = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderProducts",
                c => new
                    {
                        Order_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Order_Id, t.Product_Id })
                .ForeignKey("dbo.Orders", t => t.Order_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.Order_Id)
                .Index(t => t.Product_Id);
            
            AddColumn("dbo.Categories", "CategoryIcon", c => c.String());
            AddColumn("dbo.Products", "BrandOfProduct_Id", c => c.Int());
            AddColumn("dbo.Orders", "IncrementID", c => c.String());
            AddColumn("dbo.Orders", "UpdatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "Customer_Id", c => c.Int());
            CreateIndex("dbo.Products", "BrandOfProduct_Id");
            CreateIndex("dbo.Orders", "Customer_Id");
            AddForeignKey("dbo.Products", "BrandOfProduct_Id", "dbo.Brands", "Id");
            AddForeignKey("dbo.Orders", "Customer_Id", "dbo.Customers", "Id");
            DropTable("dbo.ProductOrders");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProductOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Qty = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        Order_Id = c.Int(),
                        Product_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.OrderProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.OrderProducts", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Orders", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Products", "BrandOfProduct_Id", "dbo.Brands");
            DropIndex("dbo.OrderProducts", new[] { "Product_Id" });
            DropIndex("dbo.OrderProducts", new[] { "Order_Id" });
            DropIndex("dbo.Orders", new[] { "Customer_Id" });
            DropIndex("dbo.Products", new[] { "BrandOfProduct_Id" });
            DropColumn("dbo.Orders", "Customer_Id");
            DropColumn("dbo.Orders", "UpdatedAt");
            DropColumn("dbo.Orders", "IncrementID");
            DropColumn("dbo.Products", "BrandOfProduct_Id");
            DropColumn("dbo.Categories", "CategoryIcon");
            DropTable("dbo.OrderProducts");
            DropTable("dbo.Customers");
            DropTable("dbo.Brands");
            CreateIndex("dbo.ProductOrders", "Product_Id");
            CreateIndex("dbo.ProductOrders", "Order_Id");
            AddForeignKey("dbo.ProductOrders", "Product_Id", "dbo.Products", "Id");
            AddForeignKey("dbo.ProductOrders", "Order_Id", "dbo.Orders", "Id");
        }
    }
}
