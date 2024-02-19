using MVCBlog.Core.Resources;
using System.ComponentModel.DataAnnotations;

namespace MVCBlog.Website.Models.OutputModels.Society
{
    public class SocInfViewModels
    {
        [Required]
        [Display(Name = nameof(Labels.InfList), ResourceType = typeof(Labels))]
        public SocInfList InfList { get; set; }
    }

    public enum SocInfList
    {
        [Display(Name = nameof(Labels.AllPartners), ResourceType = typeof(Labels))]
        AllPartners = 1,
        [Display(Name = nameof(Labels.Debtors), ResourceType = typeof(Labels))]
        Debtors = 2,
    }
}