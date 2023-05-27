namespace BNetInstaller.Endpoints;

internal sealed class ProductEndpoint : BaseEndpoint<ProductModel>
{
    public ProductEndpoint(string endpoint, AgentClient client) : base(endpoint, client)
    {
    }

    public static ProductEndpoint CreateFromResponse(JsonNode content, AgentClient client)
    {
        var responseURI = content["response_uri"]?.GetValue<string>();

        if (string.IsNullOrEmpty(responseURI))
            return null;

        return new(responseURI.TrimStart('/'), client);
    }
}
