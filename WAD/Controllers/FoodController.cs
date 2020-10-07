using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WAD.Models;

namespace WAD.Controllers
{
    public class FoodController : Controller
    {
        // GET: Food
        public ActionResult Index()
        {
            return View();
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