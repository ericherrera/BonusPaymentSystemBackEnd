using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Api.Models.Dtos.Responses
{
    public class PaymentDto
    {
        public PaymentDto() 
        {
            PaymentStatusList = new List<BaseResponse>();
        }

        public List<BaseResponse> PaymentStatusList { get; set; }
    }
}
