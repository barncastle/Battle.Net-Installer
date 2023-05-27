using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using BNetInstaller.Models;

namespace BNetInstaller.Endpoints.Version;

internal sealed class VersionEndpoint : BaseEndpoint<UidModel>
{
    public VersionEndpoint(AgentClient client) : base("version", client)
    {
    }

    public override async Task<JsonNode> Get()
    {
        using var response = await Client.SendAsync(Endpoint + "/" + Model.Uid, HttpMethod.Get);
        return await Deserialize(response);
    }
}
