using iTextSharp.text;
using iTextSharp.text.pdf;
using MVCBlog.Core.Database;
using MVCBlog.Core.Entities;
using MVCBlog.Core.Resources;
using MVCBlog.Website.Code;
using MVCBlog.Website.Models.OutputModels.Society;
using Palmmedia.Common.Net.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.Configuration;
using MVCBlog.Website.Properties;
using Microsoft.AspNet.Identity.Owin;

namespace MVCBlog.Website.Controllers
{
    //[Authorize(Roles = "ADMIN,OFFICE")]
    public partial class AspNetUsersController : Controller
    {
        private readonly IRepository repository;
        private string nameList = Common.List + "_" + @Common.AspNetUser;
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
        public AspNetUsersController(IRepository repository)
        {
            this.repository = repository;
        }

        public virtual async Task<ActionResult> Index(string id, int countFees = 0)
        {
            UserListViewModels viewModel = new UserListViewModels();
            if (!string.IsNullOrEmpty(id))
            {
                viewModel.ListAspNetUser = await this.repository.AspNetUsers
                    .AsNoTracking()
                    .Where(u => u.Id == id)
                    .OrderBy(x => x.LastName)
                    .ToListAsync();
                viewModel.UserName = viewModel.ListAspNetUser.FirstOrDefault().UserName;
            }
            if (countFees != 0)
                ViewBag.Message = String.Format("El socio {0} se dio de baja y se le generaron {1} cuota/s.", viewModel.UserName, countFees);

            return View(viewModel);
        }

        [HttpPost]
        public virtual async Task<ActionResult> Find(UserListViewModels viewModel)
        {
            if (ModelState.IsValid)
            {
                var usuarios = await GetUsersByCriteria(viewModel).ToListAsync();

                if (viewModel.Rols != null)
                {
                    foreach (var item in usuarios)
                    {
                        var rolList = await UserManager.GetRolesAsync(item.Id);
                        if (viewModel.Rols.Value != Rols.Socio)
                        {
                            if (rolList.Contains(viewModel.Rols.Value.ToString()))
                                viewModel.ListAspNetUser.Add(item);
                        }
                        else
                        {
                            if (rolList.Count == 0)
                                viewModel.ListAspNetUser.Add(item);
                        }
                    }
                }
                else
                    viewModel.ListAspNetUser = usuarios;

                if (viewModel.ListAspNetUser.Count == SiteConsts.MaxRows)
                    ModelState.AddModelError(String.Empty, String.Format(Validation.MaxRows, SiteConsts.MaxRows));
            }
            // devolvemos una vista parcial para renderizar la grilla
            return PartialView(MVC.AspNetUsers.Views._List, viewModel.ListAspNetUser);
        }

        IQueryable<AspNetUser> GetUsersByCriteria(UserListViewModels viewModel)
        {
            var result = repository.AspNetUsers
                    .Include(c => c.Category)
                    .AsNoTracking()
                    .Where(x => (viewModel.UserNumber == null || x.UserNumber == viewModel.UserNumber) &&
                               (viewModel.UserName == null || x.UserName.Contains(viewModel.UserName)) &&
                               (viewModel.FirstName == null || x.FirstName.Contains(viewModel.FirstName)) &&
                               (viewModel.LastName == null || x.LastName.Contains(viewModel.LastName)) &&
                               (viewModel.NickName == null || x.NickName.Contains(viewModel.NickName)))
                    .OrderBy(x => x.LastName)
                    .Take(SiteConsts.MaxRows);

            return result;
        }

        public virtual async Task<ActionResult> Details(string id)
        {
            if (id == null)
                return new HttpNotFoundWithViewResult(MVC.Shared.Views.NotFound);

            AspNetUser aspNetUser = await this.repository.AspNetUsers
                .Include(c => c.Category).Include(p => p.Province).Include(l => l.Locality)
                .AsNoTracking()
                .Where(x => x.Id == id)
                .SingleAsync();

            if (aspNetUser == null)
                return new HttpNotFoundWithViewResult(MVC.Shared.Views.NotFound);

            aspNetUser.Services = db.ServiceUsers.Where(_ => _.EndDate == null).Include(s => s.Service).Include(a => a.Service.ServiceType).Where(_ => _.AspNetUserId == id).ToList();
            aspNetUser.MonthlyFees = db.SocietyMonthlyFees.Where(_ => _.AspNetUserId == id && _.IsClosed).ToList();

            return View(aspNetUser);
        }

        public virtual async Task<FileResult> ExportDetails(string id)
        {
            if (id == null)
                RedirectToAction(MVC.Shared.Views.NotFound);

            AspNetUser aspNetUser = await this.repository.AspNetUsers
                .Include(c => c.Category).Include(p => p.Province).Include(l => l.Locality)
                .AsNoTracking()
                .Where(x => x.Id == id)
                .SingleAsync();

            if (aspNetUser == null)
                RedirectToAction(MVC.Shared.Views.NotFound);

            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                var filename = "UsrDet" + aspNetUser.UserName + "_" + DateTime.Now.ToString("mmssfff") + ".pdf";

                Document pdfDoc = new Document(PageSize.A4, 30f, 30f, 30f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                var titleFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
                pdfDoc.Add(new Paragraph("Detalles Usuario", titleFont));
                pdfDoc.Add(Chunk.NEWLINE);

                var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL);
                var phrase = new Phrase();

                #region Datos

                phrase.Add(new Chunk(Labels.UserName + ": ", normalFont));
                phrase.Add(new Chunk(aspNetUser.UserName, normalFont));
                phrase.Add(Chunk.NEWLINE);
                phrase.Add(new Chunk(Labels.FirstName + ": ", normalFont));
                phrase.Add(new Chunk(aspNetUser.FirstName, normalFont));
                phrase.Add(Chunk.NEWLINE);
                phrase.Add(new Chunk(Labels.LastName + ": ", normalFont));
                phrase.Add(new Chunk(aspNetUser.LastName, normalFont));
                phrase.Add(Chunk.NEWLINE);
                phrase.Add(new Chunk(Labels.NickName + ": ", normalFont));
                phrase.Add(new Chunk(aspNetUser.NickName, normalFont));
                phrase.Add(Chunk.NEWLINE);
                if (aspNetUser.UserNumber.HasValue && aspNetUser.Birthdate.HasValue && aspNetUser.CategoryId.HasValue)
                {
                    phrase.Add(new Chunk(Labels.UserNumber + ": ", normalFont));
                    int userNumber = aspNetUser.UserNumber == null ? 0 : Convert.ToInt32(aspNetUser.UserNumber);
                    phrase.Add(new Chunk(userNumber.ToString("000000"), normalFont));
                    phrase.Add(Chunk.NEWLINE);
                    phrase.Add(new Chunk(Labels.Birthdate + ": ", normalFont));
                    DateTime birthdate = aspNetUser.Birthdate == null ? DateTime.MinValue : Convert.ToDateTime(aspNetUser.Birthdate);
                    phrase.Add(new Chunk(birthdate.ToString("dd/MM/yyyy"), normalFont));
                    phrase.Add(Chunk.NEWLINE);
                    phrase.Add(new Chunk(Labels.Category + ": ", normalFont));
                    phrase.Add(new Chunk(aspNetUser.Category.Name, normalFont));
                    phrase.Add(Chunk.NEWLINE);
                }
                phrase.Add(new Chunk(Labels.EmailLabel + ": ", normalFont));
                phrase.Add(new Chunk(aspNetUser.Email, normalFont));
                phrase.Add(Chunk.NEWLINE);
                phrase.Add(new Chunk(Labels.PhoneNumber + ": ", normalFont));
                phrase.Add(new Chunk(aspNetUser.PhoneNumber, normalFont));
                phrase.Add(Chunk.NEWLINE);
                phrase.Add(new Chunk(Labels.Province + ": ", normalFont));
                phrase.Add(new Chunk(aspNetUser.Province.Name, normalFont));
                phrase.Add(Chunk.NEWLINE);
                phrase.Add(new Chunk(Labels.Locality + ": ", normalFont));
                phrase.Add(new Chunk(aspNetUser.Locality.Name, normalFont));
                phrase.Add(Chunk.NEWLINE);
                phrase.Add(new Chunk(Labels.Address + ": ", normalFont));
                phrase.Add(new Chunk(aspNetUser.Address, normalFont));
                phrase.Add(Chunk.NEWLINE);
                phrase.Add(new Chunk(Labels.Modified + ": ", normalFont));
                phrase.Add(new Chunk(aspNetUser.Modified.ToString(), normalFont));
                phrase.Add(Chunk.NEWLINE);
                phrase.Add(new Chunk(Labels.Modified + ": ", normalFont));
                phrase.Add(new Chunk(aspNetUser.ModifiedUser, normalFont));
                phrase.Add(Chunk.NEWLINE);

                #endregion

                pdfDoc.Add(phrase);
                string serverMapPath = Server.MapPath("~/" + aspNetUser.RelativePath + aspNetUser.Photo);
                bool exists = System.IO.File.Exists(serverMapPath);
                if (exists)
                {
                    var logo = iTextSharp.text.Image.GetInstance(serverMapPath);
                    pdfDoc.Add(logo);
                }

                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", filename);
            }
        }

        public virtual async Task<ActionResult> Edit(string id)
        {
            if (id == null)
                return new HttpNotFoundWithViewResult(MVC.Shared.Views.NotFound);

            AspNetUser aspNetUser = await repository.AspNetUsers.Where(x => x.Id == id).SingleAsync();
            if (aspNetUser == null)
                return new HttpNotFoundWithViewResult(MVC.Shared.Views.NotFound);

            List<Category> lCategory = repository.Categories.AsNoTracking().Where(x => x.Id == aspNetUser.CategoryId).OrderBy(x => x.Name).ToList();
            ViewBag.CategoryId = new SelectList(lCategory, "Id", "Name", aspNetUser.CategoryId);

            List<Province> lProvince = repository.Provinces.AsNoTracking().OrderBy(x => x.Name).ToList();
            ViewBag.ProvinceId = new SelectList(lProvince, "Id", "Name", aspNetUser.ProvinceId);

            List<Locality> lLocality = repository.Localities.AsNoTracking().Where(x => x.ProvinceId == aspNetUser.ProvinceId).OrderBy(x => x.Name).ToList();
            ViewBag.LocalityId = new SelectList(lLocality, "Id", "Name", aspNetUser.LocalityId);

            if (aspNetUser.State != AspNetUserState.Disabled && lCategory.Where(c => c.Price > 0).Any())
            {
                var lastFee = await repository.SocietyMonthlyFees.AsNoTracking()
                                                            .Where(x => x.AspNetUserId == aspNetUser.Id)
                                                            .OrderByDescending(o => o.Period)
                                                            .FirstOrDefaultAsync();
                if (lastFee == null)
                    ModelState.AddModelError("CustomError", String.Format("Este socio nunca pagó una cuota."));
                else
                    ModelState.AddModelError("CustomError", String.Format("La última cuota paga del socio corresponde al período {0:dd/MM/yyyy}.", lastFee.Period));
            }

            return View(aspNetUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Edit(AspNetUser aspNetUser, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                #region Photo
                /* if you save out as JPEG you will have only RGB: width * height * 3 = size in bytes ==> 97200 
                   155000 lo puse por poner una medida no significa nada */
                if (image != null && image.ContentLength > 0 && image.ContentLength < 155000)
                {
                    var content = new byte[image.ContentLength];
                    image.InputStream.Read(content, 0, image.ContentLength);
                    int indexOfLastDot = image.FileName.LastIndexOf('.');
                    string extension = image.FileName.Substring(indexOfLastDot + 1, image.FileName.Length - indexOfLastDot - 1);

                    if (extension == "jpg" || extension == "jpeg")
                    {
                        #region New Photo
                        Random r = new Random();
                        string newPhoto = String.Format("{0}_{1}", aspNetUser.UserName, r.Next(0, 99));
                        string newPath = System.IO.Path.Combine(aspNetUser.FullPath, String.Format("{0}.{1}", newPhoto, extension));
                        #endregion

                        #region Delete Old Photo
                        if (!String.IsNullOrEmpty(aspNetUser.Photo))
                        {
                            string oldPhoto = System.IO.Path.Combine(aspNetUser.FullPath, aspNetUser.Photo);
                            if (System.IO.File.Exists(oldPhoto))
                                System.IO.File.Delete(oldPhoto);
                        }
                        #endregion

                        aspNetUser.Photo = String.Format("{0}.{1}", newPhoto, extension);
                        image.SaveAs(newPath);
                    }
                }
                #endregion

                if (Validate(aspNetUser))
                {
                    int countFees = 0;
                    #region Socio Disable
                    //Si no se encuentra Habilitado, genera cuotas con monto 0, serían las cuotas que le falto pagar
                    var isDisabled = await repository.AspNetUsers.AsNoTracking().Where(d => d.Id == aspNetUser.Id && d.State == AspNetUserState.Disabled).AnyAsync();
                    if (!isDisabled && aspNetUser.State == AspNetUserState.Disabled && aspNetUser.UserNumber != null)
                    {
                        DateTime currentPeriod = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        var LastFee = await repository.SocietyMonthlyFees.AsNoTracking()
                                                                    .Where(x => x.AspNetUserId == aspNetUser.Id)
                                                                    .OrderByDescending(o => o.Period)
                                                                    .FirstOrDefaultAsync();
                        DateTime lastPeriod = SiteMethods.GetFirstDayOfMonth(Convert.ToDateTime(aspNetUser.Modified));
                        if (LastFee?.Period != null)
                            lastPeriod = LastFee.Period;
                        if (lastPeriod != currentPeriod)
                        {
                            while (lastPeriod <= currentPeriod)
                            {
                                SocietyMonthlyFee newFee = new SocietyMonthlyFee()
                                {
                                    CreatedUser = User.Identity.Name,
                                    AspNetUserId = aspNetUser.Id,
                                    Period = lastPeriod,
                                    Amount = 0,
                                    PaymentDate = DateTime.Now,
                                    IsClosed = false,
                                    Active = true
                                };
                                repository.SocietyMonthlyFees.Add(newFee);
                                await repository.SaveChangesAsync();

                                lastPeriod = lastPeriod.AddMonths(1);
                                countFees++;
                            }
                        }
                    }
                    #endregion

                    if (!aspNetUser.UserNumber.HasValue)
                    {
                        aspNetUser.Birthdate = null;
                        aspNetUser.CategoryId = null;
                    }


                    aspNetUser.Modified = DateTime.Now;
                    aspNetUser.ModifiedUser = User.Identity.Name;

                    repository.Entry(aspNetUser).State = EntityState.Modified;
                    await repository.SaveChangesAsync();

                    return RedirectToAction(MVC.AspNetUsers.Index(aspNetUser.Id, countFees));
                }
            }

            ViewBag.CategoryId = new SelectList(repository.Categories, "Id", "Name", aspNetUser.CategoryId);
            ViewBag.LocalityId = new SelectList(repository.Localities, "Id", "Name", aspNetUser.LocalityId);
            ViewBag.ProvinceId = new SelectList(repository.Provinces, "Id", "Name", aspNetUser.ProvinceId);
            return View(aspNetUser);
        }

        /// <summary>
        /// Obtiene las Localidades de una Provincia
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual JsonResult GetLocalities(string id)
        {
            Guid ProvinceId = new Guid(id);
            List<Locality> lLocality = this.repository.Localities.AsNoTracking()
                .Where(x => x.ProvinceId == ProvinceId).OrderBy(x => x.Name).ToList();

            return Json(new SelectList(lLocality, "Id", "Name"));
        }

        /// <summary>
        /// Obtiene las Categorias de una Persona (la idea es que siempre retorne una categoria)
        /// </summary>
        /// <param name="birthdate"></param>
        /// <returns></returns>
        public virtual JsonResult GetCategories(string id)
        {
            DateTime birthdate;
            List<Category> lCategory = new List<Category>();
            if (DateTime.TryParse(id, out birthdate))
            {
                Int32 age = SiteMethods.GetAge(birthdate, DateTime.Now);
                lCategory = repository.Categories.AsNoTracking()
                                                 .Where(x => x.YearFrom <= age && x.YearTo >= age).ToList();
            }

            return Json(new SelectList(lCategory, "Id", "Name"));
        }

        /// <summary>
        /// Actualiza las categorias de los usuarios activos/suspendidos.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ActionResult> UpdateCategories()
        {
            int update = 0;
            var lCategories = await this.repository.Categories.AsNoTracking().ToListAsync();
            if (lCategories.Count == 0)
                return Json(new { success = true, msg = Validation.ThereAreNoCategories });

            var lUsers = await this.repository.AspNetUsers.AsNoTracking()
                .Where(u => u.UserNumber.HasValue && u.Birthdate.HasValue && u.CategoryId.HasValue && u.State != AspNetUserState.Disabled).ToListAsync();
            foreach (var item in lUsers)
            {
                Int32 age = SiteMethods.GetAge(item.Birthdate, DateTime.Now);
                Category category = lCategories.Where(x => x.YearFrom <= age && x.YearTo >= age).FirstOrDefault();
                if (category?.Price > 0)
                {
                    AspNetUser user = await this.repository.AspNetUsers.Where(u => u.Id == item.Id).FirstOrDefaultAsync();
                    if (user.CategoryId != category.Id)
                    {
                        user.CategoryId = category.Id;
                        repository.Entry(user).State = EntityState.Modified;
                        await this.repository.SaveChangesAsync();
                        update++;
                    }
                }
            }
            return Json(new { success = true, msg = String.Format(Validation.TheyWereUpdated, update, lUsers.Count()) });
        }

        /// <summary>
        /// Actualiza las categorias de los usuarios activos/suspendidos.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ActionResult> GetLastNumberPartner()
        {
            int userNumber = 0;
            AspNetUser user = await this.repository.AspNetUsers.AsNoTracking().Where(u => u.UserNumber.HasValue).OrderByDescending(m => m.UserNumber).FirstOrDefaultAsync();
            if (user?.UserNumber != null)
                userNumber = Convert.ToInt32(user.UserNumber) + 1;

            return Json(new { success = true, number = userNumber });
        }

        /// <summary>
        /// Realiza todas las validaciones
        /// </summary>
        /// <param name="aspNetUser"></param>
        /// <returns></returns>
        private Boolean Validate(AspNetUser aspNetUser)
        {
            Boolean flag = true;
            if (aspNetUser.UserNumber.HasValue && (!aspNetUser.Birthdate.HasValue || !aspNetUser.CategoryId.HasValue))
            {
                ModelState.AddModelError("CustomError", Validation.BirthdateCategoryNotNull);
                flag = false;
            }

            if (aspNetUser.UserNumber.HasValue)
            {
                AspNetUser oAspNetUser = repository.AspNetUsers.AsNoTracking()
                    .Where(x => x.Id != aspNetUser.Id && x.UserNumber == aspNetUser.UserNumber)
                    .FirstOrDefault();
                if (oAspNetUser != null)
                {
                    ModelState.AddModelError("CustomError", String.Format(Validation.RepeatedPartner, oAspNetUser.UserNumber, oAspNetUser.UserName));
                    flag = false;
                }
            }

            return flag;
        }

        [HttpPost]
        public virtual JsonResult Export(UserListViewModels viewModel)
        {
            var result = GetUsersByCriteria(viewModel).ToList();
            string pathName = ConfigurationManager.AppSettings["ExportFilesPath"] + nameList + "_" + DateTime.Now.ToString("ssfff") + ".xlsx";
            string serverMapPath = Server.MapPath("~/" + pathName);

            FileInfo file = new FileInfo(serverMapPath);

            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(serverMapPath);
            }

            using (var package = new ExcelPackage(file))
            {
                var ws = package.Workbook.Worksheets.Add(Common.List + " " + Common.AspNetUser);

                var j = 1;
                var cell = ws.Cells;

                cell[1, 1].Value = Labels.LastName;
                cell[1, 2].Value = Labels.FirstName;
                cell[1, 3].Value = Labels.NickName;
                cell[1, 4].Value = Labels.UserName;
                cell[1, 5].Value = Labels.UserNumber;
                cell[1, 6].Value = Labels.NameBrief;
                cell[1, 7].Value = Labels.State;

                var i = 2;
                foreach (var user in result)
                {
                    j = 1;
                    cell[i, j++].Value = user.FirstName;
                    cell[i, j++].Value = user.LastName;
                    cell[i, j++].Value = user.NickName;
                    cell[i, j++].Value = user.UserName;
                    cell[i, j++].Value = user.UserNumber;
                    cell[i, j++].Value = user.Category?.NameBrief;
                    cell[i, j++].Value = user.State;
                    i++;
                }
                package.Save();
            }

            return Json(new
            {
                result = serverMapPath,
                success = true
            });
        }

        public virtual FileResult Download(string file)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(file);
            var fileDownloadName = nameList + ".xlsx";
            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(fileBytes, contentType, fileDownloadName);
        }
    }
}
