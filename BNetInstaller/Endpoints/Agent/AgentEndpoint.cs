using System;
using System.Threading.Tasks;
using BNetInstaller.Constants;
using Newtonsoft.Json.Linq;

namespace BNetInstaller.Endpoints.Agent;

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
        return await Deserialize(response);
    }

    protected override void ValidateResponse(JToken response, string content)
    {
        var token = response.Value<string>("authorization");

        if (string.IsNullOrEmpty(token))
            throw new Exception("Agent Error: Unable to authenticate.", new(content));

        Requester.SetAuthorization(token);
        base.ValidateResponse(response, content);
    }
}
