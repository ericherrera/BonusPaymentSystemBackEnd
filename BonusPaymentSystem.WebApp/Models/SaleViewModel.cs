using BonusPaymentSystem.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonusPaymentSystem.WebApp.Models
{
    public class SaleViewModel : Sale
    {
        public SaleViewModel(){
            CampaingsActived=  new Dictionary<int, string>();
         }

        public Dictionary<int, string> CampaingsActived { get; set; }
    }
}
