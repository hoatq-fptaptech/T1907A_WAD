namespace WAD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrderProduct2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderProducts", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.OrderProducts", "Product_Id", "dbo.Products");
            DropIndex("dbo.OrderProducts", new[] { "Order_Id" });
            DropIndex("dbo.OrderProducts", new[] { "Product_Id" });
            DropPrimaryKey("dbo.OrderProducts");
            AddColumn("dbo.OrderProducts", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.OrderProducts", "Qty", c => c.Int(nullable: false));
            AddColumn("dbo.OrderProducts", "Price", c => c.Double(nullable: false));
            AlterColumn("dbo.OrderProducts", "Order_Id", c => c.Int());
            AlterColumn("dbo.OrderProducts", "Product_Id", c => c.Int());
            AddPrimaryKey("dbo.OrderProducts", "Id");
            CreateIndex("dbo.OrderProducts", "Order_Id");
            CreateIndex("dbo.OrderProducts", "Product_Id");
            AddForeignKey("dbo.OrderProducts", "Order_Id", "dbo.Orders", "Id");
            AddForeignKey("dbo.OrderProducts", "Product_Id", "dbo.Products", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.OrderProducts", "Order_Id", "dbo.Orders");
            DropIndex("dbo.OrderProducts", new[] { "Product_Id" });
            DropIndex("dbo.OrderProducts", new[] { "Order_Id" });
            DropPrimaryKey("dbo.OrderProducts");
            AlterColumn("dbo.OrderProducts", "Product_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.OrderProducts", "Order_Id", c => c.Int(nullable: false));
            DropColumn("dbo.OrderProducts", "Price");
            DropColumn("dbo.OrderProducts", "Qty");
            DropColumn("dbo.OrderProducts", "Id");
            AddPrimaryKey("dbo.OrderProducts", new[] { "Order_Id", "Product_Id" });
            CreateIndex("dbo.OrderProducts", "Product_Id");
            CreateIndex("dbo.OrderProducts", "Order_Id");
            AddForeignKey("dbo.OrderProducts", "Product_Id", "dbo.Products", "Id", cascadeDelete: true);
            AddForeignKey("dbo.OrderProducts", "Order_Id", "dbo.Orders", "Id", cascadeDelete: true);
        }
    }
}
