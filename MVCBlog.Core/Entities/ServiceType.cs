using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVCBlog.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MVCBlog.Core.Entities
{
    public class ServiceType : EntityBase
    {
        [StringLength(50)]
        [Required]
        [Display(Name = nameof(Labels.ServiceTypeName), ResourceType = typeof(Labels))]
        public string Name { get; set; }

    }
}
