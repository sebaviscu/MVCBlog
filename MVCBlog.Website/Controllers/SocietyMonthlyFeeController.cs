using MVCBlog.Core.Database;
using MVCBlog.Core.Entities;
using MVCBlog.Core.Resources;
using MVCBlog.Website.Code;
using MVCBlog.Website.Models;
using MVCBlog.Website.Models.OutputModels.Society;
using Palmmedia.Common.Net.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVCBlog.Website.Controllers
{
    [Authorize(Roles = "ADMIN,OFFICE")]
    public partial class SocietyMonthlyFeeController : Controller
    {
        private IRepository _repository;
        private DatabaseContext db = new DatabaseContext();

        public SocietyMonthlyFeeController(IRepository repository)
        {
            _repository = repository;
        }

        public virtual async Task<ActionResult> Index(string userId)
        {
            SMFViewModels viewModel = new SMFViewModels();
            viewModel.DateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            viewModel.DateTo = viewModel.DateFrom;

            if (!string.IsNullOrEmpty(userId))
            {
                viewModel.ListSocietyMonthlyFee = await this._repository.SocietyMonthlyFees.Where(x => x.AspNetUserId == userId)
                    .Include(u => u.AspNetUser)
                    .Take(24)
                    .AsNoTracking()
                    .OrderBy(x => x.Period)
                    .ToListAsync();
                if (viewModel.ListSocietyMonthlyFee.Count > 0)
                {
                    viewModel.UserName = viewModel.ListSocietyMonthlyFee.Select(u => u.AspNetUser.UserName).FirstOrDefault();
                    viewModel.DateFrom = viewModel.ListSocietyMonthlyFee.Select(d => d.Period).Min();
                    viewModel.DateTo = viewModel.ListSocietyMonthlyFee.Select(d => d.Period).Max();
                }
            }
            return View(viewModel);
        }

        [HttpPost]
        public virtual async Task<ActionResult> Find(SMFViewModels viewModel)
        {
            if (ModelState.IsValid && ValidateFilter(viewModel))
            {
                //var too = db.SocietyMonthlyFees.Where(x => x.Active && (viewModel.UserName == null || x.AspNetUser.UserName.Contains(viewModel.UserName)) &&
                //                (x.Period >= viewModel.DateFrom && x.Period < viewModel.DateTo.AddDays(1))).OrderBy(x => x.Period).ToList();


                viewModel.DateTo = viewModel.DateTo.AddDays(1);
                viewModel.ListSocietyMonthlyFee = await this._repository.SocietyMonthlyFees
                    .Include(u => u.AspNetUser)
                    .AsNoTracking()
                    .Where(x => x.Active && (viewModel.UserName == null || x.AspNetUser.UserName.Contains(viewModel.UserName)) &&
                                (x.Created >= viewModel.DateFrom && x.Created < viewModel.DateTo))
                    .OrderBy(x => x.Period)
                    .Take(SiteConsts.MaxRows)
                    .ToListAsync();

                if (viewModel.ListSocietyMonthlyFee.Count == SiteConsts.MaxRows)
                    ModelState.AddModelError(String.Empty, String.Format(Validation.MaxRows, SiteConsts.MaxRows));
            }
            // devolvemos una vista parcial para renderizar la grilla
            return PartialView(MVC.SocietyMonthlyFee.Views._List, viewModel.ListSocietyMonthlyFee);
        }

        public virtual async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SocietyMonthlyFee societyMonthlyFee = await _repository.SocietyMonthlyFees.AsNoTracking()
                                                            .Include(u => u.AspNetUser)
                                                            .FirstOrDefaultAsync(u => u.Id == id);
            if (societyMonthlyFee == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(societyMonthlyFee);
        }

        public virtual async Task<ActionResult> Create(string id)
        {
            ViewBag.FullName = "";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DateTime birthdate;
            SocietyMonthlyFee smf = await _repository.SocietyMonthlyFees.AsNoTracking().Where(x => x.AspNetUserId == id)
                                                        .Include(u => u.AspNetUser)
                                                        .OrderByDescending(x => x.Period)
                                                        .FirstOrDefaultAsync();
            List<Category> lCategory = await this._repository.Categories.AsNoTracking().ToListAsync();
            if (smf == null)
            {
                AspNetUser user = await _repository.AspNetUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                ViewBag.FullName = user.FullName;

                if (user.CategoryId == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                DateTime period = user.Modified.HasValue ? Convert.ToDateTime(user.Modified) : DateTime.Now;
                smf = new SocietyMonthlyFee();
                smf.AspNetUserId = user.Id;
                smf.Period = new DateTime(period.Year, period.Month, 1);
                birthdate = Convert.ToDateTime(user.Birthdate);

            }
            else
            {
                smf.Period = smf.Period.AddMonths(1);
                birthdate = Convert.ToDateTime(smf.AspNetUser.Birthdate);
            }
            smf.PaymentDate = DateTime.Now;
            // Categoria/Precio
            Int32 age = SiteMethods.GetAge(birthdate, smf.Period);
            smf.Amount = lCategory.Where(x => x.YearFrom <= age && x.YearTo >= age).FirstOrDefault().Price;

            var serUser = db.ServiceUsers.Include(s => s.AspNetUser).Include(s => s.Service).Where(_ => _.AspNetUserId == id).ToList();

            foreach (var item in serUser.Where(_=>_.EndDate != null))
            {
                smf.Amount += item.Service.Amount;
            }


            if (smf.Amount == 0)
                ModelState.AddModelError("CustomError", Validation.AmountWithCero);

            return View(smf);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Create(SocietyMonthlyFee monthlyFee)
        {
            if (ModelState.IsValid && Validate(monthlyFee))
            {
                monthlyFee.Id = Guid.NewGuid();
                monthlyFee.Modified = null;
                monthlyFee.ModifiedUser = null;
                monthlyFee.CreatedUser = User.Identity.Name;
                monthlyFee.Active = true;
                _repository.SocietyMonthlyFees.Add(monthlyFee);
                await _repository.SaveChangesAsync();
                return RedirectToAction(MVC.SocietyMonthlyFee.Index(monthlyFee.AspNetUserId));
            }
            else
            {
                AspNetUser user = await _repository.AspNetUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Id == monthlyFee.AspNetUserId);
                ViewBag.FullName = user.FullName;
            }
            return View(monthlyFee);
        }

        public virtual async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SocietyMonthlyFee societyMonthlyFee = await _repository.SocietyMonthlyFees.AsNoTracking()
                                                            .Include(u => u.AspNetUser)
                                                            .FirstOrDefaultAsync(_ => _.Id == id);

            if (societyMonthlyFee == null)
                return new HttpNotFoundWithViewResult(MVC.Shared.Views.NotFound);

            return View(societyMonthlyFee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Edit(SocietyMonthlyFee monthlyFee)
        {
            if (ModelState.IsValid && Validate(monthlyFee))
            {
                monthlyFee.Modified = DateTime.Now;
                monthlyFee.ModifiedUser = User.Identity.Name;
                if (Validate(monthlyFee))
                {
                    _repository.Entry(monthlyFee).State = EntityState.Modified;
                    await _repository.SaveChangesAsync();
                    return RedirectToAction(MVC.SocietyMonthlyFee.Index(monthlyFee.AspNetUserId));
                }
            }
            else
            {
                AspNetUser user = await _repository.AspNetUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Id == monthlyFee.AspNetUserId);
                ViewBag.FullName = user.FullName;
            }

            return View(monthlyFee);
        }

        public virtual async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SocietyMonthlyFee societyMonthlyFee = await _repository.SocietyMonthlyFees.AsNoTracking()
                .Include(u => u.AspNetUser)
                .FirstOrDefaultAsync(_ => _.Id == id);
            if (societyMonthlyFee == null)
            {
                return HttpNotFound();
            }
            return View(societyMonthlyFee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            SocietyMonthlyFee societyMonthlyFee = await _repository.SocietyMonthlyFees.AsNoTracking()
                                                            .Include(u => u.AspNetUser)
                                                            .FirstOrDefaultAsync(u => u.Id == id);
            if (societyMonthlyFee.IsClosed)
            {
                ModelState.AddModelError("CustomError", Validation.IsClosed);
                return View(societyMonthlyFee);
            }

            societyMonthlyFee.Modified = DateTime.Now;
            societyMonthlyFee.ModifiedUser = User.Identity.Name;
            societyMonthlyFee.Active = false;
            _repository.Entry(societyMonthlyFee).State = EntityState.Modified;
            await _repository.SaveChangesAsync();

            return RedirectToAction(MVC.SocietyMonthlyFee.Index());
        }

        public virtual async Task<ActionResult> UpdateFeesWithClose()
        {
            int update = 0;
            var lFees = await this._repository.SocietyMonthlyFees.AsNoTracking().
                                                Where(f => f.Amount > 0 && f.Active && f.IsClosed == false).
                                                ToListAsync();
            foreach (var item in lFees)
            {
                var monthlyFee = await this._repository.SocietyMonthlyFees.FirstAsync(f => f.Id == item.Id);
                if (monthlyFee != null)
                {
                    monthlyFee.IsClosed = true;
                    _repository.Entry(monthlyFee).State = EntityState.Modified;
                    await _repository.SaveChangesAsync();
                    update++;
                }
            }
            return Json(new { success = true, msg = String.Format(Validation.TheyWereUpdated, update, lFees.Count()) });
        }

        /// <summary>
        /// Realiza todas las validaciones
        /// </summary>
        /// <param name="inVal"></param>
        /// <returns></returns>
        private Boolean ValidateFilter(SMFViewModels inVal)
        {
            Boolean flag = true;

            if (inVal.DateFrom > inVal.DateTo)
            {
                ModelState.AddModelError(String.Empty, Validation.DateFromDateTo);
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// Realizamos todas las validaciones
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        private Boolean Validate(SocietyMonthlyFee inVal)
        {
            Boolean flag = true;
            DateTime today = DateTime.Now;
            //var user = _repository.AspNetUsers.Where(u => u.Id == inVal.AspNetUserId && u.State != AspNetUserState.Disabled).AsNoTracking().FirstOrDefault();
            //if (user == null)
            //{
            //    ModelState.AddModelError("CustomError", Validation.PayActiveOrSuspended);
            //    flag = false;
            //}

            if (inVal.PaymentDate > today)
            {
                ModelState.AddModelError("CustomError", String.Format(Validation.PaymentDayUpperToToday, today));
                flag = false;
            }

            var fees = _repository.SocietyMonthlyFees.Where(f => f.IsClosed).OrderByDescending(f => f.PaymentDate).FirstOrDefault();
            if (fees != null && fees.PaymentDate > inVal.PaymentDate)
            {
                ModelState.AddModelError("CustomError", String.Format(Validation.PaymentDateLower, fees.PaymentDate));
                flag = false;
            }

            return flag;
        }

        #region barcode

        public virtual JsonResult PayWithBarcode(string barcode)
        {
            if (barcode.Trim() == string.Empty)
                return Json(barcode);

            if (barcode.Length != 12)
                return Json("Error: el código de barras no es correcto");

            int calDV = SiteMethods.GetDV(barcode.Substring(0, 11));
            if (calDV < 0)
                return Json("Error: no se pudo calcular el digito verificador");

            int inDV = Convert.ToInt32(barcode.Substring(11, 1));
            if (inDV != calDV)
                return Json("Error: el digito verificador no es correcto");

            int tipo = Convert.ToInt32(barcode.Substring(0, 1));
            int anio = Convert.ToInt32("20" + barcode.Substring(1, 2));
            int mes = Convert.ToInt32(barcode.Substring(3, 2));
            if (mes < 1 || mes > 12)
                return Json("Error: el mes no es correcto" + mes.ToString());

            int nrosoc = Convert.ToInt32(barcode.Substring(5, 6));
            DateTime period = new DateTime(anio, mes, 1);

            var user = _repository.AspNetUsers.Where(u => u.UserNumber == nrosoc).FirstOrDefault();
            if (user == null)
                return Json("Error: el socio no existe");

            var cat = _repository.Categories.Where(c => c.Id == user.CategoryId).FirstOrDefault();
            if (cat == null)
                return Json("Error: la categoría no existe");

            // Ultimo periodo pago
            var pay = _repository.SocietyMonthlyFees.Where(p => p.AspNetUserId == user.Id).OrderByDescending(p => p.Period).FirstOrDefault();
            if (pay != null)
            {
                if (pay.Period == period && pay.Amount == 0)
                    return Json(string.Format("Error: este período figura como adeudado para {0} - {1}/{2} - ${3:n}", user.FullName, mes, anio, cat.Price));
                else if (pay.Period == period)
                    return Json(string.Format("Error: este período ya esta pago para {0} - {1}/{2} - ${3:n}", user.FullName, mes, anio, cat.Price));
                else if (pay.Period.AddMonths(1) < period)
                    return Json(string.Format("Error: solo se pueden pagar meses consecutivos, último período pago {0:yyMM}", pay.Period));
            }

            var price = cat.Price;
            var servUser = db.ServiceUsers.Include(s => s.AspNetUser).Include(s => s.Service).Where(_ => _.AspNetUserId == user.Id).ToList();

            foreach (var item in servUser.Where(_ => _.StartDate <= period && _.EndDate == null))
            {
                price += item.Service.Amount;
            }

            SocietyMonthlyFee oMonthlyFee = new SocietyMonthlyFee
            {
                CreatedUser = User.Identity.Name,
                AspNetUserId = user.Id,
                Period = period,
                Amount = price,
                PaymentDate = DateTime.Now,
                Active = true
            };
            oMonthlyFee.Id = Guid.NewGuid();

            db.SocietyMonthlyFees.Add(oMonthlyFee);
            db.SaveChanges();

            barcode = string.Format("{0} - {1}/{2} - ${3:n}", user.FullName, mes, anio, cat.Price);

            return Json(barcode);
        }

        #endregion
    }
}
