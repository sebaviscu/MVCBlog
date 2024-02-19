using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MVCBlog.Core.Resources;

namespace MVCBlog.Website.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = nameof(Labels.EmailLabel), ResourceType = typeof(Labels))]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = nameof(Labels.Code), ResourceType = typeof(Labels))]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = nameof(Labels.RememberBrowser), ResourceType = typeof(Labels))]
        public bool RememberBrowser { get; set; }

        [Display(Name = nameof(Labels.RememberMe), ResourceType = typeof(Labels))]
        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = nameof(Labels.EmailLabel), ResourceType = typeof(Labels))]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required()]
        [RegularExpression("^[0-9]{7,9}$", ErrorMessageResourceType = typeof(Validation), ErrorMessageResourceName = nameof(Validation.UserNameDNI))]
        [Display(Name = nameof(Labels.UserName), ResourceType = typeof(Labels))]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = nameof(Labels.Password), ResourceType = typeof(Labels))]
        public string Password { get; set; }

        [Display(Name = nameof(Labels.RememberMe), ResourceType = typeof(Labels))]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required()]
        [RegularExpression("^[0-9]{7,9}$", ErrorMessageResourceType = typeof(Validation), ErrorMessageResourceName = nameof(Validation.UserNameDNI))]
        [Display(Name = nameof(Labels.UserName), ResourceType = typeof(Labels))]
        public string UserName { get; set; }

        [Required()]
        [Display(Name = nameof(Labels.FirstName), ResourceType = typeof(Labels))]
        public string FirstName { get; set; }

        [Required()]
        [Display(Name = nameof(Labels.LastName), ResourceType = typeof(Labels))]
        public string LastName { get; set; }

        [Required()]
        [EmailAddress()]
        [Display(Name = nameof(Labels.EmailLabel), ResourceType = typeof(Labels))]
        public string Email { get; set; }

        [Required()]
        [StringLength(100, ErrorMessageResourceType = typeof(Validation), ErrorMessageResourceName = nameof(Validation.LeastCharacters), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = nameof(Labels.Password), ResourceType = typeof(Labels))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = nameof(Labels.ConfirmPassword), ResourceType = typeof(Labels))]
        [Compare(nameof(Labels.Password), ErrorMessageResourceType = typeof(Validation), ErrorMessageResourceName = nameof(Validation.PasswordConfirmationDoNotMatch))]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = nameof(Labels.EmailLabel), ResourceType = typeof(Labels))]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(Validation), ErrorMessageResourceName = nameof(Validation.LeastCharacters), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = nameof(Labels.Password), ResourceType = typeof(Labels))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = nameof(Labels.ConfirmPassword), ResourceType = typeof(Labels))]
        [Compare("Password", ErrorMessageResourceType = typeof(Validation), ErrorMessageResourceName = nameof(Validation.PasswordConfirmationDoNotMatch))]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = nameof(Labels.EmailLabel), ResourceType = typeof(Labels))]
        public string Email { get; set; }
    }

    public class RolesViewModel
    {
        public string Id { get; set; }
        public IList<string> SelectedRoles { get; set; }
        public IList<System.Web.Mvc.SelectListItem> AvailableRoles { get; set; }

        public RolesViewModel()
        {
            SelectedRoles = new List<string>();
            AvailableRoles = new List<System.Web.Mvc.SelectListItem>();
        }
    }
}
