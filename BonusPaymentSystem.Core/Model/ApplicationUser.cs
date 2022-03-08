using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Core.Model
{
    [Table("AspNetUsers")]
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Codigo Empleado")]
        public string EmployeeCode { get; set; }
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }
        [Display(Name = "Apellido")]
        public string LastName { get; set; }
        [Display(Name = "Numero Cuenta")]
        public string BankAccount { get; set; }
        [Display(Name = "Estado")]
        public int Status { get; set; }
    }
}
