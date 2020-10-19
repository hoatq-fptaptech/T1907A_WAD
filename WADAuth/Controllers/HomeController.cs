using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WADAuth.Context;
using WADAuth.Models;

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
            }
            catch(Exception e)
            {
                return HttpNotFound();
            }
           
            return RedirectToAction("Index");
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