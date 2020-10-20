using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WADAuth.Context;
using WADAuth.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace WADAuth.Controllers
{
    public class HomeController : Controller
    {
        private DataContext db = new DataContext();
        private ApplicationDbContext idDb = new ApplicationDbContext();
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
            UserCart userCart = new UserCart { ProductID = item.Product.Id, Quantity = item.Quantity, UserID = User.Identity.GetUserId() };
            IEnumerable<UserCart> list = idDb.UserCarts.Where(u => u.UserID == User.Identity.GetUserId()).ToList();
            int check = CheckExist(list, userCart);
            if (check >=0)
            {
                UserCart uc = idDb.UserCarts.Find();
                uc.Quantity += item.Quantity;
                idDb.Entry(uc).State = EntityState.Modified;
                idDb.SaveChanges();
            }
            else
            {
                idDb.UserCarts.Add(userCart);
                idDb.SaveChanges();
            }

        }

        private int CheckExist(IEnumerable<UserCart> list,UserCart item)
        {
            for (int  i=0; i< list.Count;i++)
            {
                if (list[i].ProductID == item.ProductID)
                    return i;
            }
            return -1;
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