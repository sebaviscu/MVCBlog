using MVCBlog.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCBlog.Core.Entities
{
    public class ServiceFee : EntityBase
    {

        //[Required]
        //public Guid ServiceUser { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = nameof(Labels.PaymentDate), ResourceType = typeof(Labels))]
        public DateTime PaymentDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = nameof(Labels.Period), ResourceType = typeof(Labels))]
        public DateTime Period { get; set; }

        public int YearPeriod => Period.Year;
        public int MonthPeriod => Period.Month;

        [Required]
        [Display(Name = nameof(Labels.IsClosed), ResourceType = typeof(Labels))]
        public bool IsClosed { get; set; }

        [Required]
        [Display(Name = nameof(Labels.Active), ResourceType = typeof(Labels))]
        public bool Active { get; set; }

        [Required]
        [Display(Name = nameof(Labels.Amount), ResourceType = typeof(Labels))]
        public decimal Amount { get; set; }

        [Display(Name = nameof(Labels.CreatedUser), ResourceType = typeof(Labels))]
        public string CreatedUser { get; set; }

        [Display(Name = nameof(Labels.ModifiedUser), ResourceType = typeof(Labels))]
        public string ModifiedUser { get; set; }

    }
}
