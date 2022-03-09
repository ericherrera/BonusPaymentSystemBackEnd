using BonusPaymentSystem.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonusPaymentSystem.WebApp.Models
{
    public class CampaingReportModelView 
    {
        public CampaingReportModelView()
        {
            Campaings = new Dictionary<int, string>();
            Sales = new List<Sale>();
        }
        public Dictionary<int, string> Campaings;

        public Campaing Campaing { get; set; }
        public IEnumerable<Sale> Sales { get; set; }

        public decimal TotalSale 
        {
            get 
            {
                return Sales.Sum(i => i.Amount.Value); 
            } 
        }
    }
}
