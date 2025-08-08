namespace BNetInstaller.Endpoints.Update;

internal sealed class UpdateEndpoint : BaseProductEndpoint<ProductPriorityModel>
{
    public UpdateEndpoint(AgentClient client) : base("update", client)
    {
        Model.Priority.Value = 699;
    }
}
