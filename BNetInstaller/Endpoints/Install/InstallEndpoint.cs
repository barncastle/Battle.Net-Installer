using System.Threading.Tasks;
using BNetInstaller.Constants;
using BNetInstaller.Models;
using Newtonsoft.Json.Linq;

namespace BNetInstaller.Endpoints.Install
{
    class InstallEndpoint : BaseEndpoint
    {
        public InstallModel Model { get; }
        public ProductEndpoint Product { get; private set; }

        public InstallEndpoint(Requester requester) : base("install", requester)
        {
            Model = new InstallModel();
        }

        public async Task<JToken> Post()
        {
            using (var response = await Requester.SendAsync(Endpoint, HttpVerb.POST, Model))
            {
                var content = await Deserialize(response);
                Product = ProductEndpoint.CreateFromResponse(content, Requester);
                return content;
            }               
        }
    }
}
