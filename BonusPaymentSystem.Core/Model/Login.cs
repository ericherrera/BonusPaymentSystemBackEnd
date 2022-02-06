using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Core.Model
{
    public class Login
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(450)]
        public string IdUser { get; set; }
        public DateTimeOffset LoginOn { get; set; }

        [MaxLength(450)]
        public string UserName { get; set; }

        [MaxLength(256)]
        public string Host { get; set; }
        public string SessionId { get; set; }
        public int Status { get; set; }

    }
}
