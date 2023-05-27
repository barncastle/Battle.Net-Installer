using System.Text.Json.Nodes;
using System.Threading.Tasks;
using BNetInstaller.Models;

namespace BNetInstaller.Endpoints;

internal abstract class BaseProductEndpoint<T> : BaseEndpoint<T> where T : class, IModel, new()
{
    public ProductEndpoint Product { get; private set; }

    protected BaseProductEndpoint(string endpoint, AgentClient client) : base(endpoint, client)
    {
    }

    public override async Task<JsonNode> Post()
    {
        var content = await base.Post();
        Product = ProductEndpoint.CreateFromResponse(content, Client);
        return content;
    }
}
