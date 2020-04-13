using System;
using System.Threading.Tasks;
using BNetInstaller.Constants;
using Newtonsoft.Json.Linq;

namespace BNetInstaller.Endpoints.Agent
{
    internal class AgentEndpoint : BaseEndpoint
    {
        public AgentEndpoint(Requester requester) : base("agent", requester)
        {
        }

        public async Task Delete()
        {
            await Requester.SendAsync(Endpoint, HttpVerb.DELETE);
        }

        public async Task<JToken> Get()
        {
            using var response = await Requester.SendAsync(Endpoint, HttpVerb.GET);
            var content = await Deserialize(response);

            var token = content.Value<string>("authorization");
            if (string.IsNullOrEmpty(token))
                throw new Exception("Unable to authorise");

            Requester.SetAuthorization(token);
            return content;
        }
    }
}
