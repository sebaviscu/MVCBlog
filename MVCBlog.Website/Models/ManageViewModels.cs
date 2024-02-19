using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MVCBlog.Core.Entities;
using MVCBlog.Core.Resources;

namespace MVCBlog.Website.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }

        public AspNetUser User { get; set; }

        public List<Service> Services { get; set; }

        public List<SocietyMonthlyFee> MonthlyFees { get; set; }

    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(Validation), ErrorMessageResourceName = nameof(Validation.LeastCharacters), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = nameof(Labels.Password), ResourceType = typeof(Labels))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = nameof(Labels.ConfirmPassword), ResourceType = typeof(Labels))]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(Validation), ErrorMessageResourceName = nameof(Validation.PasswordConfirmationDoNotMatch))]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = nameof(Labels.CurrentPassword), ResourceType = typeof(Labels))]
        public string OldPassword { get; set; }

        [Required()]
        [StringLength(100, ErrorMessageResourceType = typeof(Validation), ErrorMessageResourceName = nameof(Validation.LeastCharacters), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = nameof(Labels.Password), ResourceType = typeof(Labels))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = nameof(Labels.ConfirmPassword), ResourceType = typeof(Labels))]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(Validation), ErrorMessageResourceName = nameof(Validation.PasswordConfirmationDoNotMatch))]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}