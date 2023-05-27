namespace BNetInstaller.Endpoints;

internal abstract class BaseEndpoint<T> where T : class, IModel, new()
{
    public string Endpoint { get; }
    public T Model { get; }

    protected AgentClient Client { get; }

    protected BaseEndpoint(string endpoint, AgentClient client)
    {
        Endpoint = endpoint;
        Client = client;
        Model = new();
    }

    public virtual async Task<JsonNode> Get()
    {
        using var response = await Client.SendAsync(Endpoint, HttpMethod.Get);
        return await Deserialize(response);
    }

    public virtual async Task<JsonNode> Post()
    {
        if (Model is NullModel)
            return default;

        using var response = await Client.SendAsync(Endpoint, HttpMethod.Post, Model);
        return await Deserialize(response);
    }

    public virtual async Task<JsonNode> Put()
    {
        if (Model is NullModel)
            return default;

        using var response = await Client.SendAsync(Endpoint, HttpMethod.Put, Model);
        return await Deserialize(response);
    }

    public virtual async Task Delete()
    {
        await Client.SendAsync(Endpoint, HttpMethod.Delete, Model);
    }

    protected async Task<JsonNode> Deserialize(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonNode.Parse(content);
        ValidateResponse(result, content);
        return result;
    }

    protected virtual void ValidateResponse(JsonNode response, string content)
    {
        var errorCode = response["error"]?.GetValue<float?>();
        if (errorCode > 0)
            throw new Exception($"Agent Error: {errorCode}", new(content));
    }
}
