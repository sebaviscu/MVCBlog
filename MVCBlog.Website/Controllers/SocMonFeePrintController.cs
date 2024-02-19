using iTextSharp.text;
using iTextSharp.text.pdf;
using MVCBlog.Core.Database;
using MVCBlog.Core.Entities;
using MVCBlog.Website.Code;
using MVCBlog.Website.Models.OutputModels.Society;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVCBlog.Website.Controllers
{
    [Authorize(Roles = "ADMIN,OFFICE")]
    public partial class SocMonFeePrintController : Controller
    {
        private IRepository _repository;
        private DatabaseContext db = new DatabaseContext();

        public SocMonFeePrintController(IRepository repository)
        {
            _repository = repository;
        }

        public virtual async Task<ActionResult> Index()
        {
            SMFPViewModels viewModel = new SMFPViewModels();
            viewModel.FirstSemester = true;
            viewModel.Year = DateTime.Now.Year;
            viewModel.ListSocietyMonthlyFee = await this._repository.SocietyMonthlyFees.AsNoTracking()
                .OrderBy(x => x.PaymentDate)
                .ToListAsync();

            return View(viewModel);
        }

        [HttpPost]
        public virtual FileResult Export(SMFPViewModels viewModel)
        {

            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                var filename = "Error_" + DateTime.Now.ToString("ssfff") + ".pdf";

                if (ModelState.IsValid && ValidateFilter(viewModel))
                {
                    var qAspNetUser = this._repository.AspNetUsers.AsNoTracking()
                        .Where(x => (viewModel.UserName == null || x.UserName.Contains(viewModel.UserName)) &&
                                    x.State != AspNetUserState.Disabled && x.Birthdate.HasValue && x.UserNumber.HasValue);

                    var lCategory = this._repository.Categories.AsNoTracking().ToList();
                    var cat = new Category();

                    var serUsers = db.ServiceUsers.Include(s => s.AspNetUser).Include(s => s.Service);

                    //int from = viewModel.FirstSemester ? 1 : 7;
                    //int to = from + 6;

                    var month = viewModel.Months.GetHashCode();
                    int from = month;
                    int to = month == 12 ? 1 : month + 1;

                    filename = "Cuotas" + viewModel.Year.ToString() + "_" + DateTime.Now.ToString("ssfff") + ".pdf";
                    Document pdfDoc = new Document(PageSize.A4, 20f, 20f, 30f, 0f);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    #region User Fees
                    foreach (var user in qAspNetUser)
                    {
                        if (HasAnyQuota(from, to, viewModel.Year, Convert.ToDateTime(user.Birthdate), lCategory))
                        {
                            var servUser = serUsers.Where(_ => _.AspNetUserId == user.Id).ToList();

                            pdfDoc.NewPage();
                            // Encabezado
                            PdfPTable table = new PdfPTable(3);
                            PdfPCell cellHeader = new PdfPCell(new Phrase(String.Format("Año: {0}\nNro: {1:000000}\nSocio: {2} Usr.DNI: {3}", viewModel.Year, user.UserNumber, user.FullName, user.UserName), new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellHeader.Colspan = 3;
                            cellHeader.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cellHeader.VerticalAlignment = 1;
                            cellHeader.MinimumHeight = 60;
                            table.AddCell(cellHeader);

                            // Cuotas
                            var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK);
                            for (int i = to - 1; i >= from; i--)
                            {
                                // Categoria/Precio
                                DateTime periodo = new DateTime(viewModel.Year, i, 1);
                                Int32 age = SiteMethods.GetAge(user.Birthdate, periodo);
                                cat = lCategory.Where(x => x.YearFrom <= age && x.YearTo >= age).FirstOrDefault();
                                if (cat.Price > 0)
                                {
                                    // Datos Socio
                                    table.AddCell(new Phrase(GetQuota(periodo, user, cat, servUser), normalFont));
                                    table.AddCell(new Phrase(GetQuota(periodo, user, cat, servUser), normalFont));
                                    table.AddCell(new Phrase(GetQuota(periodo, user, cat, servUser), normalFont));

                                    // Codigo de Barras
                                    PdfContentByte cb = writer.DirectContent;
                                    Barcode128 bc = new Barcode128();
                                    bc.TextAlignment = Element.ALIGN_CENTER;
                                    bc.Code = SiteMethods.GetBarcodeDV(1, viewModel.Year, i, user.UserNumber);
                                    bc.StartStopText = false;
                                    bc.CodeType = Barcode128.CODE128;
                                    bc.Extended = true;
                                    iTextSharp.text.Image PatImage1 = bc.CreateImageWithBarcode(cb, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.BaseColor.BLACK);
                                    PatImage1.ScaleToFit(320, 40);

                                    PdfPCell cellBarcode = new PdfPCell(PatImage1);
                                    cellBarcode.HorizontalAlignment = 1;
                                    cellBarcode.VerticalAlignment = 1;

                                    table.AddCell(cellBarcode);
                                    table.AddCell("Cobrador");
                                    table.AddCell("Socio");
                                }
                            }
                            pdfDoc.Add(table);
                        }
                    }
                    #endregion

                    // esta pagina en blanco es para que no tire error si no creamos ninguna página
                    var phrase = new Phrase();
                    phrase.Add(new Chunk(qAspNetUser.Count() == 0 ? String.Format("El Usuario DNI {0} no existe.", viewModel.UserName) : "."));
                    pdfDoc.Add(phrase);

                    pdfDoc.Close();
                }
                return File(stream.ToArray(), "application/pdf", filename);
            }
        }

        /// <summary>
        /// Realiza todas las validaciones
        /// </summary>
        /// <param name="inVal"></param>
        /// <returns></returns>
        private Boolean ValidateFilter(SMFPViewModels inVal)
        {
            Boolean flag = true;

            return flag;
        }

        private string GetQuota(DateTime period, AspNetUser user, Category cat, List<ServiceUser> serviceUsers)
        {
            string category = "error";
            decimal price = -1;
            if (cat != null)
            {
                category = cat.NameBrief;
                price = cat.Price;

                foreach (var item in serviceUsers.Where(_ => _.StartDate < DateTime.Now && _.EndDate == null))
                {
                    price += item.Service.Amount;
                }

            }

            return string.Format("\nPeríodo: {0:yyMM}\nNro: {1:000000}\nSoc: {2}\nCat.: {3} ${4:n}\n.", period, user.UserNumber, user.FullName, category, price); ;
        }

        private bool HasAnyQuota(int from, int to, int year, DateTime birthdate, List<Category> lCategory)
        {
            Category cat = new Category();
            bool flag = false;
            for (int c = from; c < to; c++)
            {
                // Categoria/Precio
                DateTime periodo = new DateTime(year, c, 1);
                Int32 age = SiteMethods.GetAge(birthdate, periodo);
                cat = lCategory.Where(x => x.YearFrom <= age && x.YearTo >= age).FirstOrDefault();
                if (cat.Price > 0)
                    flag = true;
            }
            return flag;
        }
    }
}