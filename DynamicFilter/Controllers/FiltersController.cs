using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;
using DynamicFilter.DTO;
using DynamicFilter.Models;
using Microsoft.Ajax.Utilities;

namespace DynamicFilter.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FiltersController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Filters
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

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
            ViewBag.CategoryID = new SelectList(db.Categories.Where(x => x.Enable == true), "CategoryID", "Name");
            ViewBag.TypeID = new SelectList(db.Types.Where(x => x.Enable == true), "TypeID", "Name");
            ViewBag.StateID = new SelectList(db.States.Where(x => x.Enable == true), "StateID", "Name");

            return View();
        }

        // POST: Filters/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FilterID,Description,Place,Detail,CategoryID,TypeID,StateID")] Models.Filter filter)
        {
            if (ModelState.IsValid)
            {
                filter.Enable = true;
                filter.CreatedOn = DateTime.Today;
                filter.CreatedBy = Convert.ToInt32(Session["UserID"]);
                filter.StateID = 1;
                db.Filters.Add(filter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", filter.CategoryID);
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "Name", filter.TypeID);
            ViewBag.StateID = new SelectList(db.States, "StateID", "Name", filter.StateID);
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
            ViewBag.StateID = new SelectList(db.States.Where(x => x.Enable == true), "StateID", "Name", filter.StateID);
            return View(filter);
        }

        // POST: Filters/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FilterID,Description,Place,Detail,CategoryID,TypeID,StateID")] Models.Filter filter,
            string enviar
            )
        {
            if (ModelState.IsValid)
            {
                Models.Filter model = db.Filters.Find(filter.FilterID);

                model.Description = filter.Description;
                model.Place = filter.Place;
                model.Detail = filter.Detail;
                model.CategoryID = filter.CategoryID;
                model.TypeID = filter.TypeID;
                if(enviar=="Save")
                {
                    model.StateID = 2;
                }
                else
                {
                    model.StateID = 3;
                }
                                
                db.Entry(model).State = EntityState.Modified;                                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories.Where(x=>x.Enable==true), "CategoryID", "Name", filter.CategoryID);
            ViewBag.TypeID = new SelectList(db.Types.Where(x=>x.Enable==true), "TypeID", "Name", filter.TypeID);
            ViewBag.StateID = new SelectList(db.States.Where(x => x.Enable == true), "StateID", "Name", filter.StateID);
            
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
            
            int UserId = Session["UserID"]==null ?0: Convert.ToInt32(Session["UserID"]);
            int RoleId = Convert.ToInt32(Session["RoleID"]);

            var list = db.Filters.Include("Category").Include("Type").Include("State").Include("User").
                    Where(x => x.Enable == true 
                    &&( x.CreatedBy==UserId)
                    )
                    .OrderByDescending(x=>x.FilterID) .ToList();
              
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
