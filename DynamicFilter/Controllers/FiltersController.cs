using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DynamicFilter.Models;

namespace DynamicFilter.Controllers
{
    public class FiltersController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Filters
        public ActionResult Index()
        {
            var filters = db.Filters.Include(f => f.Category).Include(f => f.Type).Where(x => x.Enable == true);
            return View(filters.ToList());
        }

        // GET: Filters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Filter filter = db.Filters.Find(id);
            if (filter == null)
            {
                return HttpNotFound();
            }
            return View(filter);
        }

        // GET: Filters/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories.Where(x=>x.Enable==true), "CategoryID", "Name");
            ViewBag.TypeID = new SelectList(db.Types.Where(x => x.Enable == true), "TypeID", "Name");
            return View();
        }

        // POST: Filters/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FilterID,Description,Place,Detail,CategoryID,TypeID")] Models.Filter filter)
        {
            if (ModelState.IsValid)
            {
                filter.Enable = true;
                filter.CreatedOn = DateTime.Today;
                db.Filters.Add(filter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", filter.CategoryID);
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "Name", filter.TypeID);
            return View(filter);
        }

        // GET: Filters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Filter filter = db.Filters.Find(id);
            if (filter == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories.Where(x => x.Enable == true), "CategoryID", "Name", filter.CategoryID);
            ViewBag.TypeID = new SelectList(db.Types.Where(x => x.Enable == true), "TypeID", "Name", filter.TypeID);
            return View(filter);
        }

        // POST: Filters/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FilterID,Description,Place,Detail,CategoryID,TypeID")] Models.Filter filter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(filter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories.Where(x=>x.Enable==true), "CategoryID", "Name", filter.CategoryID);
            ViewBag.TypeID = new SelectList(db.Types.Where(x=>x.Enable==true), "TypeID", "Name", filter.TypeID);
            return View(filter);
        }

        // GET: Filters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Filter filter = db.Filters.Include(f => f.Category).Include(f => f.Type).Where(x => x.FilterID == id).SingleOrDefault();
            if (filter == null)
            {
                return HttpNotFound();
            }
            return View(filter);
        }

        // POST: Filters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Models.Filter filter = db.Filters.Find(id);
            //db.Filters.Remove(filter);
            filter.Enable = false;
            db.Entry(filter).State = EntityState.Modified;
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



        [HttpPost]
        public JsonResult Get()
        {

            var list = db.Filters.Include("Category").Include("Type").Where(x => x.Enable == true).ToList();
            return Json(new
            {
                draw = 1,
                recordsTotal = list.Count,
                recordsFiltered = list.Count,
                data = list
            });
        }
    }
}
