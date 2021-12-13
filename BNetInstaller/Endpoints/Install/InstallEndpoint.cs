using System;
using System.Threading.Tasks;
using BNetInstaller.Constants;
using BNetInstaller.Models;
using Newtonsoft.Json.Linq;

namespace BNetInstaller.Endpoints.Install;

internal class InstallEndpoint : BaseEndpoint
{
    public InstallModel Model { get; }
    public ProductEndpoint Product { get; private set; }

    public InstallEndpoint(Requester requester) : base("install", requester)
    {
        Model = new();
    }

    public async Task<JToken> Post()
    {
        using var response = await Requester.SendAsync(Endpoint, HttpVerb.POST, Model);
        var content = await Deserialize(response);
        Product = ProductEndpoint.CreateFromResponse(content, Requester);
        return content;
    }

    protected override void ValidateResponse(JToken response, string content)
    {
        var agentError = response.Value<float?>("error");

        if (agentError > 0)
        {
            // try to identify the erroneous section
            foreach (var section in SubSections)
            {
                var token = response["form"]?[section];
                var errorCode = token?.Value<float?>("error");
                if (errorCode > 0)
                    throw new Exception($"Agent Error: Unable to install - {errorCode} ({section}).", new(content));
            }

            // fallback to throwing a global error
            throw new Exception($"Agent Error: {agentError}", new(content));
        }
    }

    private static readonly string[] SubSections = new[]
    {
        "authentication",
        "game_dir",
        "min_spec"
    };
}
