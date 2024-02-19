using System;
using System.Collections.Generic;
using System.Linq;
using MVCBlog.Core.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCBlog.Core.Entities
{
    public class AspNetRoles : EntityBase
    {
        [StringLength(50)]
        [Required]
        [Display(Name = nameof(Labels.Description), ResourceType = typeof(Labels))]
        public string Name { get; set; }



    }
}
