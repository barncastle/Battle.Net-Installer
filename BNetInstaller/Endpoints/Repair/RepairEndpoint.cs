using System.Threading.Tasks;
using BNetInstaller.Constants;
using BNetInstaller.Models;
using Newtonsoft.Json.Linq;

namespace BNetInstaller.Endpoints.Repair
{
    internal class RepairEndpoint : BaseEndpoint
    {
        public ProductPriorityModel Model { get; }
        public ProductEndpoint Product { get; private set; }

        public RepairEndpoint(Requester requester) : base("repair", requester)
        {
            Model = new ProductPriorityModel();
            Model.Priority.Value = 1000;
        }

        public async Task<JToken> Get()
        {
            using var response = await Requester.SendAsync(Endpoint, HttpVerb.GET);
            return await Deserialize(response);
        }

        public async Task<JToken> Post()
        {
            using var response = await Requester.SendAsync(Endpoint, HttpVerb.POST, Model);
            var content = await Deserialize(response);
            Product = ProductEndpoint.CreateFromResponse(content, Requester);
            return content;
        }
    }
}
