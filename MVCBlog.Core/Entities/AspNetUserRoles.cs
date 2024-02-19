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

    public class AspNetUserRoles : EntityBase
    {
        public Guid RoleId { get; set; }
        public AspNetRoles Role { get; set; }

        
        public string UserId { get; set; }
        public AspNetUser User { get; set; }
    }
}
