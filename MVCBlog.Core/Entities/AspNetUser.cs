using MVCBlog.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.IO;

namespace MVCBlog.Core.Entities
{
    public class AspNetUser
    {
        #region Identity Server

        [Required]
        [Key]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessageResourceType = typeof(Validation), ErrorMessageResourceName = nameof(Validation.Email))]
        [Display(Name = nameof(Labels.EmailLabel), ResourceType = typeof(Labels))]
        public string Email { get; set; }

        [Required]
        [Display(Name = nameof(Labels.EmailConfirmed), ResourceType = typeof(Labels))]
        public bool EmailConfirmed { get; set; }

        [Display(Name = nameof(Labels.PasswordHash), ResourceType = typeof(Labels))]
        public string PasswordHash { get; set; }

        [Display(Name = nameof(Labels.SecurityStamp), ResourceType = typeof(Labels))]
        public string SecurityStamp { get; set; }

        [Display(Name = nameof(Labels.PhoneNumber), ResourceType = typeof(Labels))]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = nameof(Labels.PhoneNumberConfirmed), ResourceType = typeof(Labels))]
        public bool PhoneNumberConfirmed { get; set; }

        [Required]
        [Display(Name = nameof(Labels.TwoFactorEnabled), ResourceType = typeof(Labels))]
        public bool TwoFactorEnabled { get; set; }

        [Display(Name = nameof(Labels.LockoutEndDateUtc), ResourceType = typeof(Labels))]
        public DateTime? LockoutEndDateUtc { get; set; }

        [Required]
        [Display(Name = nameof(Labels.LockoutEnabled), ResourceType = typeof(Labels))]
        public bool LockoutEnabled { get; set; }

        [Required]
        [Display(Name = nameof(Labels.AccessFailedCount), ResourceType = typeof(Labels))]
        public int AccessFailedCount { get; set; }

        [Required]
        [Display(Name = nameof(Labels.UserName), ResourceType = typeof(Labels))]
        public string UserName { get; set; }

        #endregion

        #region Sociedad

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = nameof(Labels.Modified), ResourceType = typeof(Labels))]
        public DateTime? Modified { get; set; }

        [Display(Name = nameof(Labels.ModifiedUser), ResourceType = typeof(Labels))]
        public string ModifiedUser { get; set; }

        [RegularExpression("([0-9]+)", ErrorMessageResourceType = typeof(Validation), ErrorMessageResourceName = nameof(Validation.OnlyInteger))]
        [Range(0, 999999, ErrorMessageResourceType = typeof(Validation), ErrorMessageResourceName = nameof(Validation.NumberBetween))]
        [DisplayFormat(DataFormatString = "{0:000000}", ApplyFormatInEditMode = true)]
        [Display(Name = nameof(Labels.UserNumber), ResourceType = typeof(Labels))]
        public int? UserNumber { get; set; }

        [Required]
        [MaxLength(60)]
        [Display(Name = nameof(Labels.NameLabel), ResourceType = typeof(Labels))]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(60)]
        [Display(Name = nameof(Labels.LastName), ResourceType = typeof(Labels))]
        public string LastName { get; set; }

        [MaxLength(30)]
        [Display(Name = nameof(Labels.NickName), ResourceType = typeof(Labels))]
        public string NickName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = nameof(Labels.Birthdate), ResourceType = typeof(Labels))]
        public DateTime? Birthdate { get; set; }

        [Display(Name = nameof(Labels.Category), ResourceType = typeof(Labels))]
        public Guid? CategoryId { get; set; }
        // Haciendo esta declaracion hacemos el mapeo para mostrar el Nombre/Descripcion
        public virtual Category Category { get; set; }

        [Required]
        [Display(Name = nameof(Labels.Province), ResourceType = typeof(Labels))]
        public Guid ProvinceId { get; set; }
        public virtual Province Province { get; set; }

        [Required]
        [Display(Name = nameof(Labels.Locality), ResourceType = typeof(Labels))]
        public Guid LocalityId { get; set; }
        public virtual Locality Locality { get; set; }

        [Display(Name = nameof(Labels.Address), ResourceType = typeof(Labels))]
        public string Address { get; set; }

        [Display(Name = nameof(Labels.Photo), ResourceType = typeof(Labels))]
        public string Photo { get; set; }

        [Display(Name = nameof(Labels.State), ResourceType = typeof(Labels))]
        public AspNetUserState State { get; set; }

        [Display(Name = nameof(Labels.NameLabel), ResourceType = typeof(Labels))]
        public string FullName
        {
            get { return String.Format("{0} {1}", this.LastName, this.FirstName); }
        }

        #endregion

        [NotMapped]
        public List<ServiceUser> Services { get; set; }

        [NotMapped]
        public List<SocietyMonthlyFee > MonthlyFees { get; set; }

        [NotMapped]
        [Display(Name = "Rols")]
        public Rols Rols { get; set; }

        /// <summary>
        /// Gets the relative path.
        /// </summary>
        [NotMapped]
        public string RelativePath
        {
            get
            {
                return ConfigurationManager.AppSettings["UserPhotosPath"];
            }
        }

        /// <summary>
        /// Gets the file path.
        /// </summary>
        [NotMapped]
        public string FullPath
        {
            get
            {
                var applicationPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                return Path.Combine(applicationPath, this.RelativePath);
            }
        }
    }

    public enum AspNetUserState
    {
        [Display(Name = nameof(Labels.Active), ResourceType = typeof(Labels))]
        Active = 1,
        [Display(Name = nameof(Labels.Disabled), ResourceType = typeof(Labels))]
        Disabled = 2,
        [Display(Name = nameof(Labels.Suspended), ResourceType = typeof(Labels))]
        Suspended = 3,
    }

    public enum Rols
    {
        [Display(Name = "Admin")]
        ADMIN = 1,
        [Display(Name = "Profesor")]
        COACH = 2,
        [Display(Name = "Administrador")]
        OFFICE = 3,
        [Display(Name = "Socio")]
        Socio = 3
    }
}
