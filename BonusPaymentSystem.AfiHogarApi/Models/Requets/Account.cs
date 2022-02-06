using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.AfiHogarApi.Models.Requets
{
    public class Account
    {
        public Account()
        {
            SchemeName = "4WRD.AccountNumber";
        }

        public string SchemeName { get; set; }
        public string Identification { get; set; }
    }
}
