using System.Text.Json.Nodes;
using System.Threading.Tasks;
using BNetInstaller.Constants;
using BNetInstaller.Models;

namespace BNetInstaller.Endpoints.Version;

internal sealed class VersionEndpoint : BaseEndpoint<UidModel>
{
    public VersionEndpoint(AgentClient client) : base("version", client)
    {
    }

    public override async Task<JsonNode> Get()
    {
        using var response = await Client.SendAsync(Endpoint + "/" + Model.Uid, HttpVerb.GET);
        return await Deserialize(response);
    }
}
