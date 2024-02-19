using MVCBlog.Core.Resources;
using System.ComponentModel.DataAnnotations;

namespace MVCBlog.Website.Models.OutputModels.Society
{
    public class ServiceInfViewModels
    {
        [Required]
        [Display(Name = nameof(Labels.InfList), ResourceType = typeof(Labels))]
        public ServiceInf InfList { get; set; }
    }

    public enum ServiceInf
    {
        [Display(Name = nameof(Labels.ServiceForType), ResourceType = typeof(Labels))]
        ServiceForType = 1,
        [Display(Name = nameof(Labels.ServiceForUsersForCoach), ResourceType = typeof(Labels))]
        ServiceForUsersForCoach = 2,
    }
}