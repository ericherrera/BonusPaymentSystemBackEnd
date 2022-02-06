using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.AfiHogarApi.Models.Requets
{
    public class Transfer
    {
        public Transfer()
        {
            Risk = new Risk 
            {
                TransferContextCode = "Internal Transfer"
            };
        }

        public TransferRequest Data { get; set; }
        public Risk Risk { get; set; }
    }
}
