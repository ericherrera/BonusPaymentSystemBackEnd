using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BonusPaymentSystem.AfiHogarApi.Models.Responses
{
    public class AuthorizeTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("header")]
        public Header Header { get; set; }
    }
}
