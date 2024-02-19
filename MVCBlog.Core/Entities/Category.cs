using MVCBlog.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCBlog.Core.Entities
{
    public class Category : EntityBase
    {
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }

        [Display(Name = nameof(Labels.CreatedUser), ResourceType = typeof(Labels))]
        public string CreatedUser { get; set; }

        [Display(Name = nameof(Labels.ModifiedUser), ResourceType = typeof(Labels))]
        public string ModifiedUser { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = nameof(Labels.Category), ResourceType = typeof(Labels))]
        public string Name { get; set; }

        [Required]
        [StringLength(3)]
        [Display(Name = nameof(Labels.NameBrief), ResourceType = typeof(Labels))]
        public string NameBrief { get; set; }

        [Required]
        [RegularExpression("([0-9]+)", ErrorMessageResourceType = typeof(Validation), ErrorMessageResourceName = nameof(Validation.OnlyInteger))]
        [Range(0, 999, ErrorMessageResourceType = typeof(Validation), ErrorMessageResourceName = nameof(Validation.NoMoreCharacters))]        
        [Display(Name = nameof(Labels.YearFrom), ResourceType = typeof(Labels))]
        public Int16 YearFrom { get; set; }

        [Required]
        [RegularExpression("([0-9]+)", ErrorMessageResourceType = typeof(Validation), ErrorMessageResourceName = nameof(Validation.OnlyInteger))]
        [Range(1, 999, ErrorMessageResourceType = typeof(Validation), ErrorMessageResourceName = nameof(Validation.NoMoreCharacters))]
        [Display(Name = nameof(Labels.YearTo), ResourceType = typeof(Labels))]
        public Int16 YearTo { get; set; }

        [Required]
        [Range(0, 999999, ErrorMessageResourceType = typeof(Validation), ErrorMessageResourceName = nameof(Validation.NumberBetween))]
        [Display(Name = nameof(Labels.Price), ResourceType = typeof(Labels))]
        public decimal Price { get; set; }
    }
}
