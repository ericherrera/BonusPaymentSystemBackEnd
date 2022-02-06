using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Core.Model
{
    [Table("Parameters")]
    public class Parameter
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Valor { get; set; }
        public string DataType { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public int State { get; set; }

    }
}
