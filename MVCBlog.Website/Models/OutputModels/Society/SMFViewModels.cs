using MVCBlog.Core.Entities;
using MVCBlog.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCBlog.Website.Models.OutputModels.Society
{
    public class SMFViewModels
    {
        public string AspNetUserId { get; set; }

        [MaxLength(10, ErrorMessage = "*")]
        [Display(Name = nameof(Labels.UserName), ResourceType = typeof(Labels))]
        public string UserName { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = nameof(Labels.DateFrom), ResourceType = typeof(Labels))]
        public DateTime DateFrom { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = nameof(Labels.DateTo), ResourceType = typeof(Labels))]
        public DateTime DateTo { get; set; }

        [MaxLength(12, ErrorMessage = "*")]
        //[RegularExpression("^[0-9]{12}*$", ErrorMessage ="*")]
        [Display(Name = nameof(Labels.Barcode), ResourceType = typeof(Labels))]
        public string Barcode { get; set; }

        public List<SocietyMonthlyFee> ListSocietyMonthlyFee { get; set; }
        public SMFViewModels()
        {
            // en el contructor tenemos que inicializar la lista
            ListSocietyMonthlyFee = new List<SocietyMonthlyFee>();
        }
    }
}