using BonusPaymentSystem.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonusPaymentSystem.WebApp.Models
{
    public class SalesCampaingViewModel
    {
        public string CampaingId { get; set; }
        public Dictionary<int, string> Campaings { get; set; }
        public  List<Sale> Sales { get;set; }
    }
}
