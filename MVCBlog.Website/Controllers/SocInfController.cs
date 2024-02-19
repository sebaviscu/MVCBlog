using MVCBlog.Core.Database;
using MVCBlog.Core.Entities;
using MVCBlog.Core.Resources;
using MVCBlog.Website.Code;
using MVCBlog.Website.Models.OutputModels.Society;
using MVCBlog.Website.Properties;
using OfficeOpenXml;
using System;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace MVCBlog.Website.Controllers
{
    [Authorize(Roles = "ADMIN,OFFICE")]
    public partial class SocInfController : Controller
    {
        private readonly IRepository repository;
        private string nameInfo = Common.Info + "_" + Common.AspNetUser;

        public SocInfController(IRepository repository)
        {
            this.repository = repository;
        }

        public virtual ActionResult Index()
        {
            SocInfViewModels viewModel = new SocInfViewModels();

            return View(viewModel);
        }

        [HttpPost]
        public virtual JsonResult Export(SocInfViewModels viewModel)
        {
            JsonResult result = new JsonResult();
            switch (viewModel.InfList)
            {
                case SocInfList.AllPartners:
                    result = SocInfGet();
                    break;
                case SocInfList.Debtors:
                    result = DebtorsInfGet();
                    break;
            }
            return result;
        }

        [HttpPost]
        public virtual JsonResult ExportService(Guid id)
        {
            JsonResult result = DebtorsInfGet(); 
            //switch (viewModel.InfList)
            //{
            //    case SocInfList.AllPartners:
            //        result = SocInfGet();
            //        break;
            //    case SocInfList.Debtors:
            //        result = DebtorsInfGet();
            //        break;
            //}
            return result;
        }

        public virtual FileResult Download(string file)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(file);
            var fileDownloadName = nameInfo + ".xlsx";
            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(fileBytes, contentType, fileDownloadName);
        }

        #region Socios

        /// <summary>
        /// Listado de todos los socios con su correspondiente estado.
        /// </summary>
        /// <returns></returns>
        private JsonResult SocInfGet()
        {
            var result = repository.AspNetUsers
                        .Include(c => c.Category)
                        .Include(l => l.Locality)
                        .Include(p => p.Province)
                        .AsNoTracking()
                        .Where(x => x.UserNumber != null)
                        .OrderBy(x => x.LastName);
            string pathName = ConfigurationManager.AppSettings["ExportFilesPath"] + Labels.AllPartners.Replace(' ','_') + "_" + DateTime.Now.ToString("ssfff") + ".xlsx";
            string serverMapPath = Server.MapPath("~/" + pathName);

            FileInfo file = new FileInfo(serverMapPath);

            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(serverMapPath);
            }

            using (var package = new ExcelPackage(file))
            {
                var ws = package.Workbook.Worksheets.Add(Common.Info + " " + Labels.AllPartners);

                var j = 1;
                var cell = ws.Cells;


                cell[1, 1].Value = Labels.LastName;
                cell[1, 2].Value = Labels.FirstName;
                cell[1, 3].Value = Labels.UserName;
                cell[1, 4].Value = Labels.NickName;
                cell[1, 5].Value = Labels.Birthdate;
                cell[1, 6].Value = Labels.Category;
                cell[1, 7].Value = Labels.UserNumber;
                cell[1, 8].Value = Labels.EmailLabel;
                cell[1, 9].Value = Labels.PhoneNumber;
                cell[1, 10].Value = Labels.Province;
                cell[1, 11].Value = Labels.Locality;
                cell[1, 12].Value = Labels.Address;
                cell[1, 13].Value = Labels.State;

                var i = 2;
                foreach (var user in result)
                {
                    j = 1;
                    cell[i, j++].Value = user.LastName;
                    cell[i, j++].Value = user.FirstName;
                    cell[i, j++].Value = user.UserName;
                    cell[i, j++].Value = user.NickName;
                    cell[i, j++].Value = String.Format("{0:dd/MM/yyyy}",user.Birthdate);
                    cell[i, j++].Value = user.Category.Name;
                    cell[i, j++].Value = user.UserNumber;
                    cell[i, j++].Value = user.Email;
                    cell[i, j++].Value = user.PhoneNumber;
                    cell[i, j++].Value = user.Province.Name;
                    cell[i, j++].Value = user.Locality.Name;
                    cell[i, j++].Value = user.Address;
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

        #endregion

        #region Debtors

        /// <summary>
        /// Listado de Socios que deben una cuota o mas.
        /// </summary>
        /// <returns></returns>
        private JsonResult DebtorsInfGet()
        {
            DateTime lastPeriod = SiteMethods.GetFirstDayOfMonth(DateTime.Now);
            int countPeriod = 0;
            decimal amountFee = 0;

            var categories = repository.Categories.AsNoTracking().ToList();

            var result = repository.AspNetUsers
                        .Include(c => c.Category)
                        .AsNoTracking()
                        .Where(x => x.State != AspNetUserState.Disabled && x.Category.Price > 0)
                        .OrderBy(x => x.LastName).ToList();
            string pathName = ConfigurationManager.AppSettings["ExportFilesPath"] + Labels.Debtors.Replace(' ', '_') + "_" + DateTime.Now.ToString("ssfff") + ".xlsx";
            string serverMapPath = Server.MapPath("~/" + pathName);


            FileInfo file = new FileInfo(serverMapPath);

            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(serverMapPath);
            }

            using (var package = new ExcelPackage(file))
            {
                var ws = package.Workbook.Worksheets.Add(Common.Info + " " + Labels.Debtors);

                var j = 1;
                var cell = ws.Cells;

                cell[1, 1].Value = Labels.LastName;
                cell[1, 2].Value = Labels.FirstName;
                cell[1, 3].Value = Labels.UserName;
                cell[1, 4].Value = Labels.NickName;
                cell[1, 5].Value = Labels.Category;
                cell[1, 6].Value = Labels.UserNumber;
                cell[1, 7].Value = Labels.Price;
                cell[1, 8].Value = Labels.Quantity;
                cell[1, 9].Value = Labels.Amount;

                var i = 2;
                foreach (var user in result)
                {
                    Category cat = categories.Where(c => c.Id == user.CategoryId).First();
                    var fee = repository.SocietyMonthlyFees
                                .AsNoTracking()
                                .Where(x => x.AspNetUserId == user.Id)
                                .OrderByDescending(x => x.Period)
                                .FirstOrDefault();
                    if (fee != null)
                    {
                        countPeriod = lastPeriod.Month - fee.Period.Month;
                        amountFee = cat.Price * countPeriod;
                        if (amountFee > 0)
                        {
                            cell[i, j++].Value = user.LastName;
                            cell[i, j++].Value = user.FirstName;
                            cell[i, j++].Value = user.UserName;
                            cell[i, j++].Value = user.NickName;
                            cell[i, j++].Value = user.Category.Name;
                            cell[i, j++].Value = user.UserNumber;
                            cell[i, j++].Value = user.Category.Price;
                            cell[i, j++].Value = countPeriod;
                            cell[i, j++].Value = amountFee;
                            i++;
                        }
                    }
                    else
                        amountFee = countPeriod = 0;
                }
                package.Save();
            }

            return Json(new
            {
                result = serverMapPath,
                success = true
            });
        }

        #endregion
    }
}