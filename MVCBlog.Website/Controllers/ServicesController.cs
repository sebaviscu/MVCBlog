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
    public partial class ServicesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }

        }
        // GET: Services
        public virtual ActionResult Index()
        {
            var services = db.Services.Include(s => s.ServiceType).Include(u => u.Coach).OrderBy(_ => _.ServiceType.Name).ToList();

            //var users = db.AspNetUsers.ToList();
            //foreach (var item in services)
            //{
            //    var user = users.FirstOrDefault(_ => _.Id == item.CoachId);
            //    if (user != null)
            //        item.Coach = user;
            //}

            return View(services);
        }

        // GET: Services/Details/5
        public virtual ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Service service = db.Services.Find(id);
            Service service = db.Services.Include(s => s.Coach).Include(s => s.ServiceType).ToList().FirstOrDefault(_ => _.Id == id);

            if (service == null)
            {
                return HttpNotFound();
            }
            service.ServiceUsers = db.ServiceUsers.Include(s => s.AspNetUser).Where(_ => _.ServiceId == id && _.EndDate == null).Select(u => u.AspNetUser).ToList();
            return View(service);
        }

        // GET: Services/Create
        public virtual  ActionResult Create()
        {

            var usersCoach = new List<AspNetUser>();
            foreach (var item in db.AspNetUsers.ToList())
            {
                var rolList = ReturnRols(item.Id);
                if (rolList.Contains(Rols.COACH.ToString()))
                    usersCoach.Add(item);
            }


            ViewBag.CoachId = new SelectList(usersCoach, "Id", "FullName");
            ViewBag.ServiceTypeId = new SelectList(db.ServiceTypes, "Id", "Name");
            return View();
        }

        public List<string> ReturnRols(string id)
        {
            List<string> list = Task.Run(() => getRols(id)).Result;
            return list;
        }

        public async Task<List<string>> getRols(string id)
        {
            var rolList = await UserManager.GetRolesAsync(id);
            return rolList.ToList();
        }

        public async Task<List<AspNetUser>> GetCoachUsers()
        {
            var usesrs = new List<AspNetUser>();
            foreach (var item in db.AspNetUsers)
            {
                var rolList = await UserManager.GetRolesAsync(item.Id);

                if (rolList.Contains(Rols.COACH.ToString()))
                    usesrs.Add(item);
            }

            return usesrs;
        }


        // POST: Services/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create([Bind(Include = "Id,Description,Amount,CoachId,ServiceTypeId,SchedulerDay,SchedulerTime,Created,Modified")] Service service)
        {
            if (ModelState.IsValid)
            {
                service.Id = Guid.NewGuid();
                service.SchedulerDay = service.Scheduler.ToString();
                db.Services.Add(service);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CoachId = new SelectList(db.AspNetUsers, "Id", "FullName", service.CoachId);
            ViewBag.ServiceTypeId = new SelectList(db.ServiceTypes, "Id", "Name", service.ServiceTypeId);
            return View(service);
        }

        // GET: Services/Edit/5
        public virtual ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }

            switch (service.SchedulerDay)
            {
                case "Lunes":
                    service.Scheduler = SchedulerDay.Lunes;
                    break;
                case "Martes":
                    service.Scheduler = SchedulerDay.Martes;
                    break;
                case "Miercoles":
                    service.Scheduler = SchedulerDay.Miercoles;
                    break;
                case "Jueves":
                    service.Scheduler = SchedulerDay.Jueves;
                    break;
                case "Viernes":
                    service.Scheduler = SchedulerDay.Viernes;
                    break;
                case "Sabado":
                    service.Scheduler = SchedulerDay.Sabado;
                    break;
            }
            var usersCoach = new List<AspNetUser>();
            foreach (var item in db.AspNetUsers.ToList())
            {
                var rolList = ReturnRols(item.Id);
                if (rolList.Contains(Rols.COACH.ToString()))
                    usersCoach.Add(item);
            }

            ViewBag.CoachId = new SelectList(usersCoach, "Id", "FullName", service.CoachId);
            ViewBag.ServiceTypeId = new SelectList(db.ServiceTypes, "Id", "Name", service.ServiceTypeId);
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit([Bind(Include = "Id,Description,Amount,CoachId,ServiceTypeId,SchedulerDay,SchedulerTime,Created,Modified,Scheduler")] Service service)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service).State = EntityState.Modified;
                service.SchedulerDay = service.Scheduler.ToString();
                //service.SchedulerTime = SchedulerTimeString.
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CoachId = new SelectList(db.AspNetUsers, "Id", "FullName", service.CoachId);
            ViewBag.ServiceTypeId = new SelectList(db.ServiceTypes, "Id", "Name", service.ServiceTypeId);
            return View(service);
        }

        // GET: Services/Delete/5
        public virtual ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Include(_ => _.Coach).Include(_ => _.ServiceType).FirstOrDefault(_ => _.Id == id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public virtual ActionResult DeleteConfirmed(Guid id)
        {
            Service service = db.Services.Find(id);
            db.Services.Remove(service);
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
