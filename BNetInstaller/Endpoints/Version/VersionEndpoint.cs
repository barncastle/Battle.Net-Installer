namespace BNetInstaller.Endpoints.Version;

internal sealed class VersionEndpoint(AgentClient client) : BaseEndpoint<UidModel>("version", client)
{
    public override async Task<JsonNode> Get()
    {
        using var response = await Client.SendAsync(Endpoint + "/" + Model.Uid, HttpMethod.Get);
        return await Deserialize(response);
    }
}
