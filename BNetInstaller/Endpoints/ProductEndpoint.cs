using System.Threading.Tasks;
using BNetInstaller.Constants;
using BNetInstaller.Models;
using Newtonsoft.Json.Linq;

namespace BNetInstaller.Endpoints
{
    internal class ProductEndpoint : BaseEndpoint
    {
        public ProductModel Model { get; private set; }

        public ProductEndpoint(string endpoint, Requester requester) : base(endpoint, requester)
        {
            Model = new ProductModel();
        }

        public async Task<JToken> Get()
        {
            using var response = await Requester.SendAsync(Endpoint, HttpVerb.GET);
            return await Deserialize(response);
        }

        public async Task<JToken> Post()
        {
            using var response = await Requester.SendAsync(Endpoint, HttpVerb.POST, Model);
            return await Deserialize(response);
        }

        public static ProductEndpoint CreateFromResponse(JToken content, Requester requester)
        {
            string responseURI = content.Value<string>("response_uri");
            if (!string.IsNullOrEmpty(responseURI))
            {
                responseURI = responseURI.TrimStart('/');
                return new ProductEndpoint(responseURI, requester);
            }
            else
            {
                return null;
            }
        }
    }
}
