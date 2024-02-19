using MVCBlog.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCBlog.Core.Entities
{
    public class Locality : EntityBase
    {
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        //public virtual ICollection<Province> Provinces { get; set; }
        public Guid ProvinceId { get; set; }
        public virtual Province Province { get; set; }
        [StringLength(30)]
        [Display(Name = nameof(Labels.Locality), ResourceType = typeof(Labels))]
        public string Name { get; set; }
        [StringLength(10)]
        public string PostalCode { get; set; }
    }
}
