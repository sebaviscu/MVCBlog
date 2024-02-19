using MVCBlog.Core.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.IO;

namespace MVCBlog.Core.Entities
{
    public class SocietyMonthlyFee : EntityBase
    {
        [Display(Name = nameof(Labels.CreatedUser), ResourceType = typeof(Labels))]
        public string CreatedUser { get; set; }

        [Display(Name = nameof(Labels.ModifiedUser), ResourceType = typeof(Labels))]
        public string ModifiedUser { get; set; }

        [Required]
        public string AspNetUserId { get; set; }
        public AspNetUser AspNetUser { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = nameof(Labels.Period), ResourceType = typeof(Labels))]
        public DateTime Period { get; set; }

        [Required]
        [Display(Name = nameof(Labels.Amount), ResourceType = typeof(Labels))]
        public decimal Amount { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = nameof(Labels.PaymentDate), ResourceType = typeof(Labels))]
        public DateTime? PaymentDate { get; set; }

        [Required]
        [Display(Name = nameof(Labels.IsClosed), ResourceType = typeof(Labels))]
        public bool IsClosed { get; set; }

        [Required]
        [Display(Name = nameof(Labels.Active), ResourceType = typeof(Labels))]
        public bool Active { get; set; }

        public int YearPeriod => Period.Year;
        public int MonthPeriod => Period.Month;

        public string PeriodString => GetPeriodString();

        //public string PeriodString { get { return GetPeriodString(); } }

        string GetPeriodString()
        {
            string dev = string.Empty;

            switch (MonthPeriod)
            {
                case 1:
                    dev = "Enero";
                    break;
                case 2:
                    dev = "Febrero";
                    break;
                case 3:
                    dev = "Marzo";
                    break;
                case 4:
                    dev = "Abril";
                    break;
                case 5:
                    dev = "Mayo";
                    break;
                case 6:
                    dev = "Junio";
                    break;
                case 7:
                    dev = "Julio";
                    break;
                case 8:
                    dev = "Agosto";
                    break;
                case 9:
                    dev = "Septiembre";
                    break;
                case 10:
                    dev = "Octubre";
                    break;
                case 11:
                    dev = "Noviembre";
                    break;
                case 12:
                    dev = "Diciembre";
                    break;
            }
            return $"{dev} - {YearPeriod}";
        }
    }
}
