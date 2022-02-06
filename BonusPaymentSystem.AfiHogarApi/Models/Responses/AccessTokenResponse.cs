using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BonusPaymentSystem.AfiHogarApi.Models.Responses
{
    public class AccessTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string Token { get; set; }
        [JsonPropertyName("scope")]
        public string Scope { get; set; }
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
        [JsonPropertyName("expires_in")]
        public int TimeOut { get; set; }
    }
}
