using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BNetInstaller.Endpoints
{
    internal abstract class BaseEndpoint
    {
        public string Endpoint { get; }

        protected Requester Requester { get; }

        public BaseEndpoint(string endpoint, Requester requester)
        {
            Endpoint = endpoint;
            Requester = requester;
        }

        protected async Task<JToken> Deserialize(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JToken.Parse(content);
            ValidateResponse(result, content);
            return result;
        }

        protected virtual void ValidateResponse(JToken response, string content)
        {
            var errorCode = response.Value<float?>("error");
            if (errorCode.HasValue && errorCode.Value > 0)
                throw new Exception($"Agent Error: {errorCode}", new Exception(content));
        }
    }
}
