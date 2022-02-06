using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Core.Model
{
    [Table("Campaings")]
    public class Campaing
    {
        [Key]
        public int Id { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreateOn { get; set; }
        public string UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string Name { get; set; }
        public DateTime StartedDate { get; set; }
        public DateTime EnddDate { get; set; }
        public decimal Amount { get; set; }
        public decimal MinAllowedAmount { get; set; }
        public decimal MinAllowedRate { get; set; }
        public decimal? MaxAllowedRate { get; set; }
        public int MinTerm { get; set; }
        public int? MaxTerm { get; set; }
        public string SavingAccountSource { get; set; }
        public int State { get; set; }
        public decimal? ProfitRate { get; set; }
    }
}
