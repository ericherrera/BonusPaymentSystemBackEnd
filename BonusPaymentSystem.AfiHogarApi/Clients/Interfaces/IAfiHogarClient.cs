using BonusPaymentSystem.AfiHogarApi.Models.Requets;
using BonusPaymentSystem.AfiHogarApi.Models.Responses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.AfiHogarApi.Clients.Interfaces
{
    public interface IAfiHogarClient
    {
        Task<AccessTokenResponse> GetToken(Dictionary<string, string> headers, Dictionary<string, string> parameters);
        Task<AuthorizeTokenResponse> GetAccessTokenProvider(Dictionary<string, string> headers, Dictionary<string, string> parameters);
        Task<TransferResponse> CreateTransfer(TransferRequest transferRequest, Dictionary<string, string> headers, string url, Encoding encoding, string mediaType = "application/json");   
        Task<HttpResponseMessage> SendPostAsync(string request, string url, Encoding encoding, long? maxBuffer, string mediaType = "application/json");
        HttpResponseMessage SendPost(string request, string url, Encoding encoding, long? maxBuffer, string mediaType = "application/json");
    }
}
