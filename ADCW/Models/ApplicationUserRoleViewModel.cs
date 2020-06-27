using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ADCW.Models
{
    public class ApplicationUserRoleViewModel
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        [ForeignKey("RoleId")]
        public virtual IdentityRole Role { get; set; }
    }
}