using System.Threading.Tasks;
using BNetInstaller.Constants;
using Newtonsoft.Json.Linq;

namespace BNetInstaller.Endpoints.Game
{
    internal class GameEndpoint : BaseEndpoint
    {
        public GameEndpoint(Requester requester) : base("game", requester)
        {
        }

        public async Task<JToken> Get(string uid)
        {
            using var response = await Requester.SendAsync(Endpoint + "/" + uid, HttpVerb.GET);
            return await Deserialize(response);
        }
    }
}
