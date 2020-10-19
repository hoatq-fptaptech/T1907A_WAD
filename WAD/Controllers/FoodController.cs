using Microsoft.Ajax.Utilities;
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
        private readonly DataContext db = new DataContext();
        // GET: Food
        public ActionResult Index()
        {
            var products = db.Products.Include("CategoryOfProduct").OrderBy(p => p.Price);
           // products = products.OrderBy(p => p.ProductName);


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

        public ActionResult AddToCart(int? id)
        {
            var product = db.Products.Find(id);
           
            return RedirectToAction("Index");
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