using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Core.Model
{
    public class Audit
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset CreateOn { get; set; }
        public string InAction { get; set; }
        public string InController { get; set; }
        public string Comment { get; set; }
    }
}
