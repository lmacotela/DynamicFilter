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
using System.Net.Mail;
using System.Web.Configuration;

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
                //int UserId = Convert.ToInt32(Session["UserID"]);
                //int RoleId = Convert.ToInt32(Session["RoleID"]);

                //var filters = db.Filters
                //    .Include(f => f.Category)
                //    .Include(f => f.Type)                    
                //    .Where(x => x.Enable == 
                //    true && (x.CreatedBy == UserId || RoleId==1)
                //    ).OrderByDescending(x => x.FilterID);
                //return View(filters.ToList());
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
        public async Task<ActionResult> Create([Bind(Include = "FilterID,Description,Place,Detail,CategoryID,TypeID,StateID")] Models.Filter filter)
        {
            if (ModelState.IsValid)
            {
                var id = filter.FilterID.ToString();
                Ticket ticket = new Ticket();
                filter.Enable = true;
                filter.CreatedOn = DateTime.Today;
                filter.CreatedBy = Convert.ToInt32(Session["UserID"]);
                filter.StateID = 1;
                //ticket.Filter.Category.Name = "Prueba";

                db.Filters.Add(filter);
                await db.SaveChangesAsync();
                var list = db.Filters.Include("Category").Include("Type").Include("State").Include("User").Where(x => x.Enable == true && x.FilterID==filter.FilterID)
                    .OrderByDescending(x => x.FilterID).FirstOrDefault();
                
                await SendMail(list);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", filter.CategoryID);
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "Name", filter.TypeID);
            ViewBag.StateID = new SelectList(db.States, "StateID", "Name", filter.StateID);
           
            return View(filter);
        }

        [HttpPost]
        public async Task<JsonResult> CreateTicketAndUser(Ticket ticket)
        {
            var response = new { issuccess = false, message = ""};

            try
            {
                if (!await ValidateAndInsertOrUpdateUser(ticket.User))
                {
                    response = new{ issuccess = false, message = "Error al validar usuario"};
                    return Json(response, JsonRequestBehavior.AllowGet);
                }

                if(!await createTicket(ticket.Filter))
                {
                    response = new { issuccess = false, message = "Error al registrar consulta"};
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
               // await SendMail("Se Genero", ticket);
                response = new { issuccess = true, message = "Consulta registrada correctamente"};
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                response = new{ issuccess = false, message = ex.Message };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        private async Task<bool> createTicket(Models.Filter filter)
        {
            try
            {
                filter.Enable = true;
                filter.CreatedOn = DateTime.Today;
                filter.CategoryID = 4;
                db.Filters.Add(filter);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private async Task<bool> ValidateAndInsertOrUpdateUser(Models.User user)
        {
            try
            {
                var proveedor = db.Users.Where(x => x.UserName == user.UserName).FirstOrDefault();
                if (proveedor == null )
                {
                    user.Enable = true;
                    user.Password = string.Empty;
                    user.RoleID = 2;
                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    Models.User usuario = db.Users.Where(x => x.UserName == user.UserName).FirstOrDefault();
                    db.Entry(usuario).State = EntityState.Modified;
                    usuario.UserName = user.UserName;
                    usuario.ProveedorID = user.ProveedorID;
                    usuario.ProviderName = user.ProviderName;
                    usuario.ContactPerson = user.ContactPerson;
                    await db.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create_Ticket([Bind(Include = "Description,Place,Detail,CategoryID,TypeID,CreatedBy,StateID,UserName")] Models.Filter filter)
        {
            
           // string UserName = filter.User.UserName.ToString();
            if (ModelState.IsValid)
            {
                filter.Enable = true;
                filter.CreatedOn = DateTime.Today;
                db.Filters.Add(filter);
                await db.SaveChangesAsync();
                return new HttpStatusCodeResult(HttpStatusCode.Created);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Creacion de Metodo Create_Proveedor
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Create_Proveedor([Bind(Include = "Password, Enable, RoleID, UserName, ProveedorID")] Models.User user)
        {
            if (ModelState.IsValid)
            {
                var proveedor = db.Users.Where(x => x.UserName == user.UserName).FirstOrDefault();
                if (proveedor == null)
                {
                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                    return new HttpStatusCodeResult(HttpStatusCode.Created);
                }
                else
                {
                    Models.User usuario = db.Users.Find(proveedor.UserID);
                    usuario.Password = user.Password;
                    usuario.Enable = user.Enable;
                    usuario.RoleID = user.RoleID;
                    usuario.UserName = user.UserName;
                    usuario.ProveedorID = user.ProveedorID;
                    db.Entry(usuario).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
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


        public static async Task SendMail(Models.Filter filter)
        {
            var Mensaje = new MailMessage();
            Mensaje.To.Add(new MailAddress(WebConfigurationManager.AppSettings["UsuarioEnvio"]));
            Mensaje.From = new MailAddress(WebConfigurationManager.AppSettings["AdminUser"]);
            Mensaje.Subject = WebConfigurationManager.AppSettings["Subject"];
            var link=WebConfigurationManager.AppSettings["link"];
            string htmlString = @"<html>
                      <body>
                      <p>Dear: " + filter.User.ContactPerson.ToString() + "</p>" +
                      "<p>Ticket No.: " + filter.FilterID.ToString()+"</p>"+
                      "<p>Sugerencia Alimentos: " + filter.Category.Name.ToString() + "</p>"+
                      "<p>Category: " + filter.Type.Name.ToString() + "</p>"+
                      "<p>Estado: " + filter.State.Name.ToString() + "</p>" +
                      "<p>Asunto: " + filter.Place.ToString() + "</p>" +
                      "<p></p>" +

                      "<p></p>" +
                      "<p>Consulta: <a href='" + link.ToString()+ "' target='_blank'>Aqui</a></p>" +
                      "</body>" +
                      "</html>";
           /* string strMensaje = string.Format("Ticket No.: {1}{0}Sugerencia Alimentos: {2}{0}Category: {3}{0}Contact Name: {4}{0}",
                Environment.NewLine, filter.FilterID.ToString(),
                filter.Category.Name.ToString(), filter.Type.Name.ToString(), filter.User.ContactPerson.ToString());*/
            Mensaje.Body = htmlString;
            Mensaje.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                var credencial = new NetworkCredential
                {
                    UserName = WebConfigurationManager.AppSettings["AdminUser"],
                    Password = WebConfigurationManager.AppSettings["AdminPassword"],
                };

                smtp.Credentials = credencial;
                smtp.Host = WebConfigurationManager.AppSettings["SMTPName"];
                smtp.Port = int.Parse(WebConfigurationManager.AppSettings["SMTPPort"]);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(Mensaje);

            }
        }
            [HttpPost]
        public JsonResult Get()
        {
            
                int UserId = Session["UserID"]==null ?0: Convert.ToInt32(Session["UserID"]);
            int RoleId = Convert.ToInt32(Session["RoleID"]);

            var list = db.Filters.Include("Category").Include("Type").Include("State").Include("User").
                    Where(x => x.Enable == true 
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
