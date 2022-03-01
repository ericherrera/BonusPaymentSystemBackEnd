using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Core.Model
{
    [Table("AspNetUsers")]
    public class ApplicationUser : IdentityUser
    {
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BankAccount { get; set; }
        public int Status { get; set; }
    }
}
