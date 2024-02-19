using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using MVCBlog.Core.Database;
using MVCBlog.Core.Entities;

namespace MVCBlog.Website.Controllers
{
    public partial class ServiceUsersController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: ServiceUsers
        public virtual ActionResult Index()
        {
            var serviceUsers = db.ServiceUsers.Include(s => s.AspNetUser).Include(s => s.Service).Include(s => s.Service.ServiceType).OrderBy(_=>_.Service.Description);
            return View(serviceUsers.ToList());
        }

        // GET: ServiceUsers/Details/5
        public virtual ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var serviceUserList = db.ServiceUsers.Include(s => s.AspNetUser).Include(s => s.Service).Include(s => s.Service.ServiceType).ToList();
            var serviceUser = serviceUserList.FirstOrDefault(_ => _.Id == id);

            if (serviceUser == null)
            {
                return HttpNotFound();
            }
            return View(serviceUser);
        }

        // GET: ServiceUsers/Create
        public virtual ActionResult Create()
        {
            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "FullName");
            ViewBag.ServiceId = new SelectList(db.Services.Include(s => s.ServiceType).Include(s => s.Coach), "Id", "Titulo");
            return View();
        }


        // POST: ServiceUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create([Bind(Include = "Id,ServiceId,AspNetUserId,StartDate,EndDate,CreatedUser,ModifiedUser,Created,Modified")] ServiceUser serviceUser)
        {
            if (ModelState.IsValid)
            {
                serviceUser.Id = Guid.NewGuid();
                db.ServiceUsers.Add(serviceUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "FullName", serviceUser.AspNetUserId);
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "Description", serviceUser.ServiceId);
            return View(serviceUser);
        }

        // GET: ServiceUsers/Edit/5
        public virtual ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceUser serviceUser = db.ServiceUsers.Find(id);
            if (serviceUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "Email", serviceUser.AspNetUserId);
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "Description", serviceUser.ServiceId);
            return View(serviceUser);
        }

        // POST: ServiceUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit([Bind(Include = "Id,ServiceId,AspNetUserId,StartDate,EndDate,CreatedUser,ModifiedUser,Created,Modified")] ServiceUser serviceUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "Email", serviceUser.AspNetUserId);
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "Description", serviceUser.ServiceId);
            return View(serviceUser);
        }

        // GET: ServiceUsers/Delete/5
        public virtual ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceUser serviceUser = db.ServiceUsers.Find(id);
            if (serviceUser == null)
            {
                return HttpNotFound();
            }
            return View(serviceUser);
        }

        // POST: ServiceUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public virtual ActionResult DeleteConfirmed(Guid id)
        {
            ServiceUser serviceUser = db.ServiceUsers.Find(id);
            db.ServiceUsers.Remove(serviceUser);
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

        // GET: ServiceUsers/Edit/5
        public virtual ActionResult ListServiceUsers(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var serviceUsers = db.ServiceUsers.Include(s => s.AspNetUser).Where(_ => _.Id == id).ToList();


            return View(serviceUsers);
        }

    }
}
