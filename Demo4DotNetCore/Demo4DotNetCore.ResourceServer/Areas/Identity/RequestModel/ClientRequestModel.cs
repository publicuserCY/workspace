using Demo4DotNetCore.ResourceServer.Identity.Model;
using Demo4DotNetCore.ResourceServer.Model;

namespace Demo4DotNetCore.ResourceServer.Identity.RequestModel
{
    public class ClientRequestModel : PaginatedRequestModel
    {
        public bool? Enabled { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public Client Client { get; set; }
    }

}
