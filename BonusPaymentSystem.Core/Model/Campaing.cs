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
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Creado Por")]
        public string CreatedById { get; set; }
        [Display(Name = "Creado El")]
        public DateTime CreateOn { get; set; }
        [Display(Name = "Actualizado Por")]
        public string UpdatedById { get; set; }
        [Display(Name = "Actualizado el")]
        public DateTime? UpdatedOn { get; set; }
        [Display(Name = "Nombre de Campaña")]
        public string Name { get; set; }
        [Display(Name = "Fecha de Incio")]
        public DateTime StartedDate { get; set; }
        [Display(Name = "Fecha de Fin Por")]
        public DateTime EnddDate { get; set; }
        [Display(Name = "Monto")]
        public decimal Amount { get; set; }
        [Display(Name = "Monto Minimo Permitido")]
        public decimal MinAllowedAmount { get; set; }
        [Display(Name = "Tasa Minima Permitido")]
        public decimal MinAllowedRate { get; set; }
        [Display(Name = "Tasa Maxima Permitida")]
        public decimal? MaxAllowedRate { get; set; }
        [Display(Name = "Plazo Minimo Permitido")]
        public int MinTerm { get; set; }
        [Display(Name = "Plazo Maximo Permitido")]
        public int? MaxTerm { get; set; }
        [Display(Name = "Debitar de cuenta")]
        public string SavingAccountSource { get; set; }
        [Display(Name = "Estado")]
        public int State { get; set; }
        [Display(Name = "Porcentaje de Beneficio")]
        public decimal? ProfitRate { get; set; }
    }
}
