using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Api.Models.Dtos.Responses
{
    public class PaymentResponse
    {
        public PaymentResponse()
        {
            PaymentList = new List<PaymentDto>();
        }

       public List<PaymentDto> PaymentList { get; set; }
    }
}
