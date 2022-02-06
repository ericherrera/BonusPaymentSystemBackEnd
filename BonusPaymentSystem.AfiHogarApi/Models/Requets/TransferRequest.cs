using System;
using System.Collections.Generic;
using System.Text;

namespace BonusPaymentSystem.AfiHogarApi.Models.Requets
{
    public class TransferRequest
    {
        public TransferRequest() 
        {
            Initiation = new Initiation 
            {
                InstructionType = "Internal",
                InstructionIdentification = "df96ac00-5410-4136-8fff-3ab8ec9f1fe3",
                EndToEndIdentification = "e1c8db3f-d8cd-4c26-b0a2-e5ef153a8653"
            };

        }

        public Initiation Initiation { get; set; }
    }
}
