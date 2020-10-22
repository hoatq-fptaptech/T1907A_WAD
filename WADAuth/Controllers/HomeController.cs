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
        [Authorize]
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
           
            return RedirectToAction("Index");
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