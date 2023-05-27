using BNetInstaller.Models;

namespace BNetInstaller.Endpoints.Repair;

internal sealed class RepairEndpoint : BaseProductEndpoint<ProductPriorityModel>
{
    public RepairEndpoint(AgentClient client) : base("repair", client)
    {
        Model.Priority.Value = 1000;
    }
}
