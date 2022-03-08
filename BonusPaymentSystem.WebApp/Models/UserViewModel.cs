using BonusPaymentSystem.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BonusPaymentSystem.WebApp.Models
{
    public class UserViewModel : ApplicationUser
    {
        [Required(ErrorMessage = "Debe seleccionar un Rol")]
        [Display(Name = "Rol")]
        public string RolName { get; set; }
    }
}
