namespace BNetInstaller.Endpoints.Agent;

internal sealed class AgentEndpoint(AgentClient client) : BaseEndpoint<NullModel>("agent", client)
{
    protected override void ValidateResponse(JsonNode response, string content)
    {
        base.ValidateResponse(response, content);

        var token = response["authorization"]?.GetValue<string>();

        if (string.IsNullOrEmpty(token))
            throw new Exception("Agent Error: Unable to authenticate.", new(content));

        Client.SetAuthToken(token);
    }
}
