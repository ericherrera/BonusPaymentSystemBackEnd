using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Commons.Utilities
{
    public static class BonusCalculation
    {
        public static decimal CalcBonusPayment(decimal SaleAmount, decimal CampaingAmoung, decimal CampaingProfitAmount)
        {
            return (SaleAmount / CampaingAmoung) * CampaingAmoung * CampaingProfitAmount;
        }
    }
}
