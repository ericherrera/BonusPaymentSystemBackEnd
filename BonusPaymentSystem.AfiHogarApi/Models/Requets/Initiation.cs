using System;
using System.Collections.Generic;
using System.Text;

namespace BonusPaymentSystem.AfiHogarApi.Models.Requets
{
    public class Initiation
    {
        public string InstructionType { get; set; }
        public string InstructionIdentification { get; set; }
        public string EndToEndIdentification { get; set; }
        public InstructedAmount InstructedAmount { get; set; }
        public DebtorAccount DebtorAccount { get; set; }
        public CreditorAccount CreditorAccount { get; set; }
    }
}
