using MVCBlog.Core.Entities;
using MVCBlog.Core.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCBlog.Website.Models.OutputModels.Society
{
    public class UserListViewModels
    {
        [Display(Name = nameof(Labels.UserName), ResourceType = typeof(Labels))]
        public string UserName { get; set; }

        [Display(Name = nameof(Labels.UserNumber), ResourceType = typeof(Labels))]
        public int? UserNumber { get; set; }

        [Display(Name = nameof(Labels.FirstName), ResourceType = typeof(Labels))]
        public string FirstName { get; set; }

        [Display(Name = nameof(Labels.LastName), ResourceType = typeof(Labels))]
        public string LastName { get; set; }

        [Display(Name = nameof(Labels.NickName), ResourceType = typeof(Labels))]
        public string NickName { get; set; }

        [Display(Name = nameof(Labels.State), ResourceType = typeof(Labels))]
        public AspNetUserState? State { get; set; }

        [Display(Name = "Rol")]
        public Rols? Rols { get; set; }


        public List<AspNetUser> ListAspNetUser { get; set; }
        public UserListViewModels()
        {
            // en el contructor tenemos que inicializar la lista
            ListAspNetUser = new List<AspNetUser>();
        }
    }
}