using System;
using System.Collections.Generic;
using System.Text;

namespace BonusPaymentSystem.AfiHogarApi.Models.Requets
{
    public class CreditorAccount : Account
    {
        public CreditorAccount() 
        {
            Name = "John Smith";
        }
        public string Name { get; set; }
    }
}
