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
        [Display(Name = "Id Venta")]
        public int Id { get; set; }
        [Display(Name = "Campaña")]
        public int? CampaingId { get; set; }
        [Display(Name = "Creado el")]
        public DateTimeOffset CreatedOn { get; set; }
        [Display(Name = "Vendor Id")]
        public string UserId { get; set; }
        [Display(Name = "Vendor Id")]
        public string ReferenceCode { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Amount { get; set; }
        public int? Term { get; set; }
        public int? State { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
