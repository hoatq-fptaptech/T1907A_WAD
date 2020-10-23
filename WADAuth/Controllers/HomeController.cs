using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WADAuth.Context;
using WADAuth.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Diagnostics;
using System.Security.Claims;

namespace WADAuth.Controllers
{
    public class HomeController : Controller
    {
        private DataContext db = new DataContext();
        public ActionResult Index()
        {
            var products = db.Products;
            return View(products.ToList());
        }
        //[Authorize]
        public ActionResult AddToCart(int? id,int? qty)
        {
            try
            {
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                CartItem item = new CartItem(product, (int)qty);
                Cart cart = (Cart)Session["Cart"];
                if (cart == null)
                {
                    cart = new Cart();
                }
                cart.AddToCart(item);
                Session["Cart"] = cart;
                if(User.Identity.IsAuthenticated)
                {
                     AddToCartWithUser(item);
                }
            }
            catch(Exception e)
            {
                return HttpNotFound();
            }
           
            return RedirectToAction("Cart");
        }

        private void AddToCartWithUser(CartItem item)
        {
            string currentUserId = GetCurrentUserID();
       
            if (string.IsNullOrEmpty(currentUserId)) { return; }

            UserCart userCart = new UserCart { ProductID = item.Product.Id, Quantity = item.Quantity, UserID = User.Identity.GetUserId() };
            UserCart uc = db.UserCarts.Where(u => u.ProductID == userCart.ProductID && u.UserID == currentUserId).FirstOrDefault();
            if (uc != null)
            {
                uc.Quantity += item.Quantity;
                db.Entry(uc).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                db.UserCarts.Add(userCart);
                db.SaveChanges();
            }

        }
        private string GetCurrentUserID()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                // the principal identity is a claims identity.
                // now we need to find the NameIdentifier claim
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    return userIdClaim.Value;
                }
            }
            return null;
        }

        public ActionResult Cart()
        {
            return View();
        }

        public ActionResult RemoveItem(int? id)
        {
            Cart cart = (Cart) Session["Cart"];
            foreach(var item in cart.CartItems)
            {
                if (id == item.Product.Id)
                {
                    cart.CartItems.Remove(item);
                    if(User.Identity.IsAuthenticated)
                        RemoveItemWithUser(item);
                    break;
                }
                    
            }
            return RedirectToAction("Cart");
        }

        public void RemoveItemWithUser(CartItem item)
        {
            string currentUserId = GetCurrentUserID();
       
            if (string.IsNullOrEmpty(currentUserId)) { return; }

            UserCart userCart = new UserCart { ProductID = item.Product.Id, Quantity = item.Quantity, UserID = User.Identity.GetUserId() };
            UserCart uc = db.UserCarts.Where(u => u.ProductID == userCart.ProductID && u.UserID == currentUserId).FirstOrDefault();
            if (uc != null)
            {
                db.UserCarts.Remove(uc);
                db.SaveChanges();
            }
        }

        public ActionResult Checkout()
        {
            if (Session["Cart"] == null)
                return RedirectToAction("Cart");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceOrder([Bind(Include ="Name,Email,Telephone,Address")] PlaceOrder placeOrder) 
        {
            if (ModelState.IsValid)
            {
                Customer customer = new Customer() { CustomerName = placeOrder.Name, Email = placeOrder.Email, Address = placeOrder.Address, PhoneNumber = placeOrder.Telephone };
                db.Customers.Add(customer);
                db.SaveChanges();
                Cart cart = (Cart)Session["Cart"];
                Order order = new Order()
                {
                    IncrementID = "#" + DateTime.Now.ToBinary().ToString() + customer.Id,
                    GrandTotal = cart.GrandTotal,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Customer = customer
                };
                db.Orders.Add(order);
                db.SaveChanges();
                foreach(var item in cart.CartItems)
                {
                    OrderProduct op = new OrderProduct()
                    {
                        Qty = item.Quantity,
                        Price = item.Product.Price,
                        Order_Id = order.Id,
                        Product_Id = item.Product.Id
                    };
                    db.OrderProducts.Add(op);
                    
                }
                db.SaveChanges();

                UpdateQuantity();
                // gui email...
                
            }
            return RedirectToAction("CheckoutSuccess");
        }

        public ActionResult CheckoutSuccess()
        {
            return View();
        }

        public void UpdateQuantity()
        {
            Cart cart = (Cart)Session["Cart"];
            foreach(var item in cart.CartItems)
            {
                // thay doi so luong san pham
                Product p = db.Products.Find(item.Product.Id);
                p.Qty -= item.Quantity;
                db.Entry(p).State = EntityState.Modified;

                // xoa gio hang trong db
                if (User.Identity.IsAuthenticated)
                {
                    string userID = GetCurrentUserID();
                    if (!String.IsNullOrEmpty(userID)) {
                        UserCart uc = db.UserCarts.Where(u => u.ProductID == item.Product.Id && u.UserID == userID).FirstOrDefault();
                        if (uc != null)
                        {
                            db.UserCarts.Remove(uc);
                        }
                    }
                    
                }
            }
            db.SaveChanges();
            Session["Cart"] = null;
        }

        public double AjaxCart(int? id,int? qty) 
        {
            Cart cart = (Cart)Session["Cart"];
            foreach(var item in cart.CartItems)
            {
                if (item.Product.Id == id)
                {
                    item.Quantity = (int)qty;
                }
            }
            cart.CalculateGrandTotal();
            return cart.GrandTotal;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}