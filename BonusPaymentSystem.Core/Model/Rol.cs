using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Core.Model
{
    [Table("Rols")]
    public class Rol
    {
        [Key]
        [MaxLength(450)]
        public string Id { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string  ConcurrencyStamp { get; set; }
    }
}
