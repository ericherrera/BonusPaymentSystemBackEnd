using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Api.Models.Dtos.Responses
{
    public class BaseResponse
    {
        public int PaymentId { get; set; }
        public int CampaingId { get; set; }

        public string SallerId { get; set; }
        public int SaleId { get; set; }
        public string ReferenceCode { get; set; }
        public int Status { get; set; } = 200;
        public string Message { get; set; } = "OK";
    }
}
