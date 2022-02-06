using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Api.Models
{
    public class UserModel
    {

        [Required(ErrorMessage ="UserName is Required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
