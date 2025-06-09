namespace BNetInstaller.Endpoints;

internal abstract class BaseProductEndpoint<T>(string endpoint, AgentClient client) : BaseEndpoint<T>(endpoint, client) where T : class, IModel, new()
{
    public ProductEndpoint Product { get; private set; }

    public override async Task<JsonNode> Post()
    {
        var content = await base.Post();
        Product = ProductEndpoint.CreateFromResponse(content, Client);
        return content;
    }
}
