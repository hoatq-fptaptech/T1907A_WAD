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
using Microsoft.AspNet.Identity.EntityFramework;

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

        public void MergeCart()
        {
            // ghep gio hang giua Session va DB
            Cart cart = Session["Cart"] != null ? (Cart)Session["Cart"] : new Cart();
            DataContext db = new DataContext();
            string userID = GetCurrentUserID();
            List<UserCart> userCarts = db.UserCarts.Where(u => u.UserID == userID).ToList();
            foreach (var item in userCarts)
            {
                CartItem cartItem = new CartItem(db.Products.Find(item.ProductID), item.Quantity);
                cart.AddToCart(cartItem);
            }
            if (cart.CartItems.Count > 0)
            {
                Session.Remove("Cart");
                Session["Cart"] = cart;
                ResetUserCarts(cart.CartItems, db);
            }
         
        }

        private void ResetUserCarts(List<CartItem> cartItems, DataContext db)
        {
            string userID = GetCurrentUserID();
            var userCarts = db.UserCarts.Where(u => u.UserID == userID).ToList();
            foreach (var item in userCarts)
            {
                db.UserCarts.Remove(item);
            }
            db.SaveChanges();
            foreach (var item in cartItems)
            {
                UserCart uc = new UserCart() { ProductID = item.Product.Id, UserID = userID, Quantity = item.Quantity };
                db.UserCarts.Add(uc);
            }
            db.SaveChanges();
        }

        [Authorize]
        public ActionResult Checkout()
        {
            if (Session["Cart"] == null)
                return RedirectToAction("Cart");
            MergeCart();
            string userId = GetCurrentUserID();
           
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = manager.FindById(userId);
            PlaceOrder po = new PlaceOrder() { Name = user.UserName, Email = user.Email, Telephone = user.PhoneNumber, Address = "" };
            return View(po);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceOrder([Bind(Include ="Name,Email,Telephone,Address")] PlaceOrder placeOrder) 
        {
            if (ModelState.IsValid)
            {
                string userId = GetCurrentUserID();
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var user = manager.FindById(userId);
                Customer customer = db.Customers.Where(c => c.Email == user.Email).FirstOrDefault();
                if (customer == null)
                {
                    customer = new Customer() { CustomerName = placeOrder.Name, Email = placeOrder.Email, Address = placeOrder.Address, PhoneNumber = placeOrder.Telephone };
                    db.Customers.Add(customer);
                    db.SaveChanges();
                }
                else
                {
                    customer.CustomerName = placeOrder.Name;
                    customer.Address = placeOrder.Address;
                    customer.PhoneNumber = placeOrder.Telephone;
                    db.Entry(customer).State = EntityState.Modified;
                    db.SaveChanges();
                }
              
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
            Session.Remove("Cart");
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