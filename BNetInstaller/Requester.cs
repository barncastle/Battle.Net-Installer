using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using BNetInstaller.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BNetInstaller
{
    internal class Requester : IDisposable
    {
        public string BaseAddress { get; }

        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _serializerSettings;

        public Requester(int port)
        {
            BaseAddress = $"http://127.0.0.1:{port}/{{0}}";

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", "phoenix-agent/1.0");

            _serializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };
        }

        public void SetAuthorization(string authorization)
        {
            _client.DefaultRequestHeaders.Add("Authorization", authorization);
        }

        public async Task<HttpResponseMessage> SendAsync(string endpoint, HttpVerb verb, string content = null)
        {
            var url = string.Format(BaseAddress, endpoint);
            var request = new HttpRequestMessage(new HttpMethod(verb.ToString()), url);

            if (verb != HttpVerb.GET && !string.IsNullOrEmpty(content))
                request.Content = new StringContent(content);

            var response = await _client.SendAsync(request).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                await HandleRequestFailure(response);

            return response;
        }

        public async Task<HttpResponseMessage> SendAsync<T>(string endpoint, HttpVerb verb, T payload = null) where T : class
        {
            string content = null;
            if (payload != null)
                content = JsonConvert.SerializeObject(payload, _serializerSettings);

            return await SendAsync(endpoint, verb, content);
        }

        private async Task HandleRequestFailure(HttpResponseMessage response)
        {
            var uri = response.RequestMessage.RequestUri.AbsolutePath;
            var statusCode = response.StatusCode;
            var content = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"{(int)statusCode} {statusCode}: {uri} {content}");
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
