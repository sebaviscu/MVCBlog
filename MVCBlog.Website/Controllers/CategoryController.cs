using MVCBlog.Core.Database;
using MVCBlog.Core.Entities;
using MVCBlog.Core.Resources;
using Palmmedia.Common.Net.Mvc;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVCBlog.Website.Controllers
{
    [Authorize(Roles = "ADMIN,OFFICE")]
    public partial class CategoryController : Controller
    {
        private readonly IRepository repository;

        public CategoryController(IRepository repository)
        {
            this.repository = repository;
        }

        // GET: Category
        public virtual async Task<ActionResult> Index()
        {
            var entities = repository.Categories.AsNoTracking()
                .OrderBy(x => x.YearFrom).ToListAsync();

            return View(await entities);
        }

        // GET: Category/Details/5
        public virtual async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
                return new HttpNotFoundWithViewResult(MVC.Shared.Views.NotFound);

            Category category = await repository.Categories.AsNoTracking()
                .Where(x => x.Id == id).SingleAsync();

            if (category == null)
                return new HttpNotFoundWithViewResult(MVC.Shared.Views.NotFound);

            return View(category);
        }

        // GET: Category/Create
        public virtual ActionResult Create()
        {
            Category category = new Category();
            return View(category);
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Create(Category category)
        {
            if (ModelState.IsValid && Validate(category))
            {
                category.CreatedUser = User.Identity.Name;
                repository.Categories.Add(category);
                await repository.SaveChangesAsync();
                return RedirectToAction(MVC.Category.Index());
            }
            return View(category);
        }

        // GET: Category/Edit/5
        public virtual async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
                return new HttpNotFoundWithViewResult(MVC.Shared.Views.NotFound);

            Category category = await repository.Categories.Where(x => x.Id == id).SingleAsync();
            if (category == null)
                return new HttpNotFoundWithViewResult(MVC.Shared.Views.NotFound);

            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Modified = DateTime.Now;
                category.ModifiedUser = User.Identity.Name;
                if (Validate(category))
                {
                    repository.Entry(category).State = EntityState.Modified;
                    await repository.SaveChangesAsync();
                    return RedirectToAction(MVC.Category.Index());
                }
            }
            return View(category);
        }

        // GET: Category/Delete/5
        public virtual async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
                return new HttpNotFoundWithViewResult(MVC.Shared.Views.NotFound);

            Category category = await repository.Categories.AsNoTracking().SingleAsync(x => x.Id == id);
            if (category == null)
                return new HttpNotFoundWithViewResult(MVC.Shared.Views.NotFound);

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                Category category = repository.Categories.Find(id);
                repository.Categories.Remove(category);
                await repository.SaveChangesAsync();
                return RedirectToAction(MVC.Category.Index());
            }
            catch (Exception ex)
            {
                Category category = await repository.Categories.AsNoTracking().SingleAsync(x => x.Id == id);
                if (((System.Data.SqlClient.SqlException)ex.InnerException.InnerException).Number == 547)
                {
                    ModelState.AddModelError("CustomError", Validation.ErrorFK);
                    return View(category);
                }
                else
                    return new HttpNotFoundWithViewResult(MVC.Shared.Views.NotFound);

            }

        }

        /// <summary>
        /// Realizamos todas las validaciones
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        private Boolean Validate(Category category)
        {
            Boolean flag = true;
            if (category.YearFrom > category.YearTo)
            {
                ModelState.AddModelError("CustomError", Validation.CategoyYearFromYearTo);
                flag = false;
            }

            if (String.IsNullOrEmpty(category.ModifiedUser))
            {
                // Create
                if (repository.Categories.Where(x => (x.YearFrom >= category.YearFrom && x.YearFrom <= category.YearTo) || (x.YearTo >= category.YearFrom && x.YearTo <= category.YearTo)).Any())
                {
                    ModelState.AddModelError("CustomError", Validation.CategoryRangeExists);
                    flag = false;
                }
            }
            else
            {
                // Edit
                if (repository.Categories.Where(x => (x.Id != category.Id) && ((x.YearFrom >= category.YearFrom && x.YearFrom <= category.YearTo) || (x.YearTo >= category.YearFrom && x.YearTo <= category.YearTo))).Any())
                {
                    ModelState.AddModelError("CustomError", Validation.CategoryRangeExists);
                    flag = false;
                }
            }

            return flag;
        }
    }
}
