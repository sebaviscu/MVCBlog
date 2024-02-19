using MVCBlog.Core.Database;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVCBlog.Website.Controllers
{
    [Authorize(Roles = "ADMIN,OFFICE")]
    public partial class ProvinceController : Controller
    {
        private readonly IRepository repository;

        public ProvinceController(IRepository repository)
        {
            this.repository = repository;
        }
        
        // GET: Province
        public virtual async Task<ActionResult> Index()
        {
            return View(await repository.Provinces.ToListAsync());
        }
        /*
        // GET: Province/Details/5
        public virtual async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Province province = await db.Provinces.FindAsync(id);
            if (province == null)
            {
                return HttpNotFound();
            }
            return View(province);
        }

        // GET: Province/Create
        public virtual ActionResult Create()
        {
            return View();
        }

        // POST: Province/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Create([Bind(Include = "Id,Name,Created,Modified")] Province province)
        {
            if (ModelState.IsValid)
            {
                province.Id = Guid.NewGuid();
                db.Provinces.Add(province);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(province);
        }

        // GET: Province/Edit/5
        public virtual async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Province province = await db.Provinces.FindAsync(id);
            if (province == null)
            {
                return HttpNotFound();
            }
            return View(province);
        }

        // POST: Province/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Edit([Bind(Include = "Id,Name,Created,Modified")] Province province)
        {
            if (ModelState.IsValid)
            {
                db.Entry(province).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(province);
        }

        // GET: Province/Delete/5
        public virtual async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Province province = await db.Provinces.FindAsync(id);
            if (province == null)
            {
                return HttpNotFound();
            }
            return View(province);
        }

        // POST: Province/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Province province = await db.Provinces.FindAsync(id);
            db.Provinces.Remove(province);
            await db.SaveChangesAsync();
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
        */
    }
}
