using MVCBlog.Core.Entities;
using MVCBlog.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCBlog.Website.Models.OutputModels.Society
{
    public class SMFPViewModels
    {
        [MaxLength(10, ErrorMessage = "*")]
        [Display(Name = nameof(Labels.UserName), ResourceType = typeof(Labels))]
        public string UserName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = nameof(Labels.FirstSemester), ResourceType = typeof(Labels))]
        public bool FirstSemester { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(2000, 9999, ErrorMessage = "*")]
        [Display(Name = nameof(Labels.Year), ResourceType = typeof(Labels))]
        public int Year { get; set; }

        [Display(Name = "Mes")]
        public Months Months { get; set; }


        public List<SocietyMonthlyFee> ListSocietyMonthlyFee { get; set; }
        public SMFPViewModels()
        {
            // en el contructor tenemos que inicializar la lista
            ListSocietyMonthlyFee = new List<SocietyMonthlyFee>();
        }


    }

    public enum Months
    {
        Enero = 1,
        Febrero = 2,
        Marzo = 3,
        Abril = 4,
        Mayo = 5,
        Junio = 6,
        Julio = 7,
        Agosto = 8,
        Septiembre = 9,
        Octubre = 10,
        Noviembre = 11,
        Diciembre = 12
    }
}