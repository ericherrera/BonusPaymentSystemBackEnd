using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Core.Model
{
    [Table("RolsUsers")]
    public class RoleUser
    {
        [Key]
        public string UserId { get; set; }
        [Key]
        public string RoleId { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public int Status { get; set; }
    }
}
