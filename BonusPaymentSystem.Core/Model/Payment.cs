using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Core.Model
{
    [Table("Payments")]
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public int SaleId { get; set; }
        public string UserId { get; set; }
        public int CampaingId { get; set; }
        public decimal Amount { get; set; }
        public string SavingAccountFrom { get; set; }
        public string SavingAccountTo { get; set; }
        public string ReferenceCode { get; set; }
        public DateTimeOffset PaymentDate { get; set; }
        public int State { get; set; }
    }
}
