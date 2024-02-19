using MVCBlog.Core.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCBlog.Core.Entities
{
    public class Province : EntityBase
    {
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }

        [StringLength(30)]
        [Display(Name = nameof(Labels.Province), ResourceType = typeof(Labels))]
        public string Name { get; set; }

    }
}
