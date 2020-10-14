using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WAD.Context;
using WAD.Models;

namespace WAD.Controllers
{
    public class FoodController : Controller
    {
        private DataContext db = new DataContext();
        // GET: Food
        public ActionResult Index()
        {
            var products = db.Products.OrderBy(p=>p.ProductName);
            
            return View(products.ToList());
        }

        public ActionResult Detail(int? id)
        {
            Product product = db.Products.Find(id);
            return View(product);
        }

        public ActionResult Category(int? id)
        {
            var products = db.Products.Where(p => p.CategoryID == id);
            return View(products.ToList());
        }

        public string About()
        {
            return "about page";
        }

        public string Contact()
        {
            return "contact page";
        }
    }
}