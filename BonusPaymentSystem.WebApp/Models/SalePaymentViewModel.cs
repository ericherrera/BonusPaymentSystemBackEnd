using BonusPaymentSystem.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonusPaymentSystem.WebApp.Models
{
    public class SalePaymentViewModel
    {
        public ICollection<Sale> Sales { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
