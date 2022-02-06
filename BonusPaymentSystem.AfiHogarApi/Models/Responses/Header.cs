using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BonusPaymentSystem.AfiHogarApi.Models.Responses
{
    public class Header
    {
        [JsonPropertyName("transactionNumber")]
        public string TransactionNumber { get; set; }

        [JsonPropertyName("httpStatusCode")]
        public int HttpStatusCode { get; set; }
    }
}
