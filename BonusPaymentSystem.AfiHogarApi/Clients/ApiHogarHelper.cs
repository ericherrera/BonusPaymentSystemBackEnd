using BonusPaymentSystem.AfiHogarApi.Models.Requets;
using BonusPaymentSystem.AfiHogarApi.Models.Responses;
using BonusPaymentSystem.Commons.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.AfiHogarApi.Clients
{
    public class ApiHogarHelper
    {

        public async Task<AccessTokenResponse> GetAccessToken(string grantType = "client_credentials",
                                                              string accept = "text/html, application/json, application/xhtml+xml, */*", 
                                                              string contentType = "application/x-www-form-urlencoded",
                                                              string secret = "dzBMbkVNOUpYeWhNYmlBMEg4Nk9lM3FwVjVzYTpyRlRqanFUdkxOY25ZbTltSndHX0FNbVp6b2dh",
                                                              string url = "https://api.uat.4wrd.tech:8243/token")
        {

            var postData = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                ["grant_type"] = grantType
            });

            using var client = new HttpClient();
            postData.Headers.Clear();

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", accept);
            //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            postData.Headers.Add("Content-Type", contentType);

            client.DefaultRequestHeaders.Add("Authorization", "Bearer  " + secret);

            var result = await client.PostAsync(url, postData);
            var stringContent = await result.Content.ReadAsStringAsync();

            var oResult = JsonTool.StringJsonDeserializer<AccessTokenResponse>(stringContent);

            return oResult;
        }



        public async Task<AuthorizeTokenResponse> GetUserToken(string usrName, string pass, string accessTk, 
                                                               string password = "password",
                                                               string accept = "text/html, application/json, application/xhtml+xml, */*",
                                                               string contentType = "application/x-www-form-urlencoded",
                                                               string url = "https://api.uat.4wrd.tech:8243/authorize/2.0/token?provider=AB4WRD")
        {

            var postData = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                ["grant_type"] = password,
                ["username"] = usrName,
                ["password"] = pass
            });


            using var client = new HttpClient();
            postData.Headers.Clear();

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", accept);
            postData.Headers.Add("Content-Type", contentType);

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessTk);

            var result = await client.PostAsync(url, postData);
            var stringContent = await result.Content.ReadAsStringAsync();

            var oResult = JsonTool.StringJsonDeserializer<AuthorizeTokenResponse>(stringContent);


            return oResult;
        }

        public async Task<TransferResponse> CreateTransfer(TransferRequest transferRequest, string url, string userToken, string accessToken)
        {
            //Verificar si los headers no estan cargado y si
            using var client = new HttpClient();

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "text/html, application/json, application/xhtml+xml, */*");

            client.DefaultRequestHeaders.Add("token-id", userToken);

            //postData.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            var transfer = new Transfer
            {
                Data = transferRequest
            };

            var request = JsonTool.ObjectToJsonString(transfer);

            var content = new StringContent(content: request);
            content.Headers.ContentType.MediaType = "application/json";


           var response = await client.PostAsync(url, content);
            var stringContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Status: " + response.StatusCode + ", error Api AfiHogar message: " + stringContent);

            return JsonTool.StringJsonDeserializer<TransferResponse>(stringContent);

        }

    }
}
