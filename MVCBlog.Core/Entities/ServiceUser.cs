using MVCBlog.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Core.Entities
{
    public class ServiceUser : EntityBase
    {
        [Required]
        [Display(Name = nameof(Labels.Service), ResourceType = typeof(Labels))]
        public Guid ServiceId { get; set; }
        [Display(Name = nameof(Labels.Service), ResourceType = typeof(Labels))]
        public virtual Service Service { get; set; }

        [Required]
        [Display(Name = nameof(Labels.UserName), ResourceType = typeof(Labels))]
        public string AspNetUserId { get; set; }
        [Display(Name = nameof(Labels.UserName), ResourceType = typeof(Labels))]
        public AspNetUser AspNetUser { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = nameof(Labels.StartDate), ResourceType = typeof(Labels))]
        public DateTime StartDate { get; set; }


        [Display(Name = nameof(Labels.EndDate), ResourceType = typeof(Labels))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [Display(Name = nameof(Labels.CreatedUser), ResourceType = typeof(Labels))]
        public string CreatedUser { get; set; }

        [Display(Name = nameof(Labels.ModifiedUser), ResourceType = typeof(Labels))]
        public string ModifiedUser { get; set; }


    }
}
