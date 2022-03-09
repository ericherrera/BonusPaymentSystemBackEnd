using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Core.Model
{
    [Table("AspNetUserRoles")]
    public class RoleUser : IdentityUserRole<string>
    {
        [Key]
        [ForeignKey("Rol")]
        public override string RoleId { get => base.RoleId; set => base.RoleId = value; }
        
        [ForeignKey("ApplicationUser")]
        public override string UserId { get => base.RoleId; set => base.RoleId = value; }
    }
}
