using BonusPaymentSystem.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonusPaymentSystem.WebApp.Models
{
    public class SaleListViewModel
    {
        public bool HasOneCampaing { get; set; }
        public IEnumerable<Sale> Sales { get; set; } 
    }
}
