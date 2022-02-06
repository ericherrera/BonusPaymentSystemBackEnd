using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BonusPaymentSystem.AfiHogarApi.Models.Responses
{
    public class GeneralResponse
    {
        [JsonPropertyName("header")]
        public Header Header { get; set; }
    }
}
