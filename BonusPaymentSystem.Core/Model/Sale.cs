using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Core.Model
{
    [Table("Sales")]
    public class Sale
    {
        [Key]
        public int Id { get; set; }
        public int? CampaingId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string UserId { get; set; }
        public string ReferenceCode { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Amount { get; set; }
        public int? Term { get; set; }
        public int? State { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
