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
    public class ImageGallerysController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        //private readonly DbConnectionContext db = new DbConnectionContext();
        public ActionResult Index()
        {
            var imagesModel = new ImageGallery();
            foreach (var item in db.ImageGallerys.OrderBy(_ => _.Created))
            {
                imagesModel.ImageList.Add(item.ImagePath);
            }
            return View(imagesModel);
        }
        [HttpGet]
        public ActionResult UploadImage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadImageMethod()
        {
            if (Request.Files.Count != 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    int fileSize = file.ContentLength;
                    string fileName = file.FileName;
                    file.SaveAs(Server.MapPath("~/Upload_Files/" + fileName));
                    ImageGallery imageGallery = new ImageGallery();
                    imageGallery.Id = Guid.NewGuid();
                    imageGallery.Name = fileName;
                    imageGallery.ImagePath = "~/Upload_Files/" + fileName;
                    db.ImageGallerys.Add(imageGallery);
                    db.SaveChanges();
                }
                return Content("Success");
            }
            return Content("failed");
        }



        // GET: ImageGalleries
        //public ActionResult Index()
        //{
        //    return View(db.ImageGallerys.ToList());
        //}

        //// GET: ImageGalleries/Details/5
        //public ActionResult Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ImageGallery imageGallery = db.ImageGallerys.Find(id);
        //    if (imageGallery == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(imageGallery);
        //}

        //// GET: ImageGalleries/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: ImageGalleries/Create
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        //// más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Name,ImagePath")] ImageGallery imageGallery)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        imageGallery.Id = Guid.NewGuid();
        //        db.ImageGallerys.Add(imageGallery);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(imageGallery);
        //}

        //// GET: ImageGalleries/Edit/5
        //public ActionResult Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ImageGallery imageGallery = db.ImageGallerys.Find(id);
        //    if (imageGallery == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(imageGallery);
        //}

        //// POST: ImageGalleries/Edit/5
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        //// más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Name,ImagePath")] ImageGallery imageGallery)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(imageGallery).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(imageGallery);
        //}

        //// GET: ImageGalleries/Delete/5
        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ImageGallery imageGallery = db.ImageGallerys.Find(id);
        //    if (imageGallery == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(imageGallery);
        //}

        //// POST: ImageGalleries/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Guid id)
        //{
        //    ImageGallery imageGallery = db.ImageGallerys.Find(id);
        //    db.ImageGallerys.Remove(imageGallery);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
