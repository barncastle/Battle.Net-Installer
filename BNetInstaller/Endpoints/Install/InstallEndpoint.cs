namespace BNetInstaller.Endpoints.Install;

internal sealed class InstallEndpoint : BaseProductEndpoint<InstallModel>
{
    public InstallEndpoint(AgentClient client) : base("install", client)
    {
    }

    protected override void ValidateResponse(JsonNode response, string content)
    {
        var agentError = response["error"]?.GetValue<float?>();

        if (agentError.GetValueOrDefault() <= 0)
            return;

        // try to identify the erroneous section
        foreach (var section in new[] { "authentication", "game_dir", "min_spec" })
        {
            var node = response["form"]?[section];
            var errorCode = node?["error"]?.GetValue<float?>();

            if (errorCode > 0)
                throw new Exception($"Agent Error: Unable to install - {errorCode} ({section}).", new(content));
        }

        // fallback to throwing a global error
        throw new Exception($"Agent Error: {agentError}", new(content));
    }
}
