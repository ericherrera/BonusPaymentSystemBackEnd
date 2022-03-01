using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BonusPaymentSystem.WebApp.Models
{
    public class ChangePasswordViewModel
    {
        public string UsernName { get; set; }

        [Required(ErrorMessage = "Contraseña actual requerido.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña Actual")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Nueva contraseña requerido.")]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} y un maximo de {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva Contraseña")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Nueva contraseña requerido.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Nueva Contraseña")]
        [Compare("NewPassword", ErrorMessage = "Nueva Contraseña y confirmar nueva contraseña no coinciden.")]
        public string NewPasswordConfirm { get; set; }
    }
}
