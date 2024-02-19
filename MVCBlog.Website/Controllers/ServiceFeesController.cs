using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCBlog.Core.Database;
using MVCBlog.Core.Entities;

namespace MVCBlog.Website.Controllers
{
    public partial class ServiceFeesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: ServiceFees
        public virtual ActionResult Index()
        {
            return View(db.ServiceFees.ToList());
        }

        // GET: ServiceFees/Details/5
        public virtual ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceFee serviceFee = db.ServiceFees.Find(id);
            if (serviceFee == null)
            {
                return HttpNotFound();
            }
            return View(serviceFee);
        }

        // GET: ServiceFees/Create
        public virtual ActionResult Create()
        {
            return View();
        }

        // POST: ServiceFees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create([Bind(Include = "Id,ServiceUser,PaymentDate,Period,IsClosed,Active,Amount,CreatedUser,ModifiedUser,Created,Modified")] ServiceFee serviceFee)
        {
            if (ModelState.IsValid)
            {
                serviceFee.Id = Guid.NewGuid();
                db.ServiceFees.Add(serviceFee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(serviceFee);
        }

        // GET: ServiceFees/Edit/5
        public virtual ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceFee serviceFee = db.ServiceFees.Find(id);
            if (serviceFee == null)
            {
                return HttpNotFound();
            }
            return View(serviceFee);
        }

        // POST: ServiceFees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit([Bind(Include = "Id,ServiceUser,PaymentDate,Period,IsClosed,Active,Amount,CreatedUser,ModifiedUser,Created,Modified")] ServiceFee serviceFee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceFee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(serviceFee);
        }

        // GET: ServiceFees/Delete/5
        public virtual ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceFee serviceFee = db.ServiceFees.Find(id);
            if (serviceFee == null)
            {
                return HttpNotFound();
            }
            return View(serviceFee);
        }

        // POST: ServiceFees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public virtual ActionResult DeleteConfirmed(Guid id)
        {
            ServiceFee serviceFee = db.ServiceFees.Find(id);
            db.ServiceFees.Remove(serviceFee);
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
