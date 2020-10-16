using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WAD.Context;
using WAD.Models;
using System.Dynamic;

namespace WAD.Controllers
{
    public class CategoriesController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Categories
        public ActionResult Index()
        {
            var categories = db.Categories.ToList();
            var products = db.Products.ToList();
            ViewBag.Categories = categories;
            ViewBag.Products = products;
            //ViewData["Categories"] = categories;
            //ViewData["Products"] = products;
            dynamic data = new ExpandoObject();
            data.Categories = categories;
            data.Products = products;
            return View(data);
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CategoryName")] Category category, HttpPostedFileBase CategoryIcon)
        {
            // day file len thu muc Uploads 
            // cho url image vào CategoryIcon của category
            if(CategoryIcon != null)
            {
                string fileName = Path.GetFileName(CategoryIcon.FileName);
                string path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                CategoryIcon.SaveAs(path);// dua image len thu muc uploads
                category.CategoryIcon = "Uploads/" + fileName;
            }
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CategoryName")] Category category,HttpPostedFileBase CategoryIcon)
        {
            if(CategoryIcon != null)
            {
                string fileName = Path.GetFileName(CategoryIcon.FileName);
                string path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                CategoryIcon.SaveAs(path);
                category.CategoryIcon = "Uploads/" + fileName;
            }
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
