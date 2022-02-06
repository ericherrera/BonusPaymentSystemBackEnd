using BonusPaymentSystem.AfiHogarApi.Clients.Interfaces;
using BonusPaymentSystem.AfiHogarApi.Models.Requets;
using BonusPaymentSystem.AfiHogarApi.Models.Responses;
using BonusPaymentSystem.Commons.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.AfiHogarApi.Clients
{
    public class AfiHogarClient : IAfiHogarClient
    {
        public const string AUTORIZATION_HEADER = "Authorization";
        public const string CONTENT_TYPE_HEADER = "Content-Type";

        private readonly HttpClient _httpClient;

        private readonly string _accessTokenURL;
        private readonly string _providerUrlToken;

        public AfiHogarClient(string accessTokenUrl, string providerTokenUrl,  int timeout = 60)
        {
            _httpClient = new HttpClient();
            Headers = new Dictionary<string, string>();

            IsHeadersLoaded = false;
            _accessTokenURL = accessTokenUrl;

            _providerUrlToken = providerTokenUrl;
            _httpClient.Timeout = TimeSpan.FromSeconds(timeout);
        }

        public async Task<AccessTokenResponse> GetToken(Dictionary<string, string> headers, Dictionary<string, string> parameters) 
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();

            foreach (var header in headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            using var content = new FormUrlEncodedContent(parameters);
            var response = await httpClient.PostAsync(new Uri(_accessTokenURL), content);

            var stringContent = await response.Content.ReadAsStringAsync();

            return JsonTool.StringJsonDeserializer<AccessTokenResponse>(stringContent);


        }

        public async Task<AuthorizeTokenResponse> GetAccessTokenProvider( Dictionary<string, string> headers,  Dictionary<string, string> parameters)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();

            foreach (var header in headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }


            using var content = new FormUrlEncodedContent(parameters);
            var response = await httpClient.PostAsync(new Uri(_providerUrlToken), content);

            var stringContent = await response.Content.ReadAsStringAsync();

            return JsonTool.StringJsonDeserializer<AuthorizeTokenResponse>(stringContent);


        }

        public Dictionary<string, string> Headers { get; set; }
        private bool IsHeadersLoaded;


        private void LoadHeaders(Dictionary<string, string> headers, ref HttpRequestHeaders defaultHeader)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            foreach (var header in headers)
            {
                /*if (header.Key == CONTENT_TYPE_HEADER)
                    _httpClient.Hea.Authorization = new AuthenticationHeaderValue(header.Value);
                else*/
                defaultHeader.Add(header.Key, header.Value);

            }

        }
        private bool LoadHeaders()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            foreach (var header in Headers)
            {
               /* if (header.Key == AUTORIZATION_HEADER)
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(header.Value);
                else*/
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);

            }

            return Headers.Count > 0;
        }

        public async Task<TransferResponse> CreateTransfer(TransferRequest transferRequest, Dictionary<string, string> headers, string url, Encoding encoding, string mediaType = "application/json")
        {
            //Verificar si los headers no estan cargado y si
            IsHeadersLoaded = LoadHeaders();
            var request = JsonTool.ObjectToJsonString(transferRequest);

            using var content = new StringContent(content: request,
                                             encoding: encoding ?? Encoding.UTF8,
                                             mediaType: mediaType);

            var response = _httpClient.PostAsync(new Uri(url), content);


            var stringContent = await response.Result.Content.ReadAsStringAsync();

            return JsonTool.StringJsonDeserializer<TransferResponse>(stringContent);

        }

        public HttpResponseMessage SendPost(string request, string url, Encoding encoding, long? maxBuffer, string mediaType = "application/json")
        {
            //Verificar si los headers no estan cargado y si
            //tiene headers por cargar
            if (!IsHeadersLoaded && Headers.Count > 0)
            {
                IsHeadersLoaded = LoadHeaders();
                _httpClient.MaxResponseContentBufferSize = maxBuffer ?? _httpClient.MaxResponseContentBufferSize;
            }

            var content = new StringContent(content: request,
                                             encoding: encoding ?? Encoding.UTF8,
                                             mediaType: mediaType);

            var responseString = _httpClient.PostAsync(new Uri(url), content).Result;

            return responseString;

        }

        public async Task<HttpResponseMessage> SendPostAsync(string request, string url, Encoding encoding, long? maxBuffer, string mediaType = "application/json")
        {
            //Verificar si los headers no estan cargado y si
            //tiene headers por cargar
            if (!IsHeadersLoaded && Headers.Count > 0)
            {
                IsHeadersLoaded = LoadHeaders();
                _httpClient.MaxResponseContentBufferSize = maxBuffer ?? _httpClient.MaxResponseContentBufferSize;
            }

            var content = new StringContent(content: request,
                                             encoding: encoding ?? Encoding.UTF8,
                                             mediaType: mediaType);

            var responseString = await _httpClient.PostAsync(new Uri(url), content);

            return responseString;

        }
    }
}
