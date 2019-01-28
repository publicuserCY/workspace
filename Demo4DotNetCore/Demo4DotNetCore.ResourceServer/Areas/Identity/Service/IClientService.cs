using Demo4DotNetCore.ResourceServer.Identity.RequestModel;
using Demo4DotNetCore.ResourceServer.Model;
using System.Threading.Tasks;

namespace Demo4DotNetCore.ResourceServer.Identity.Service
{
    public interface IClientService
    {
        Task<PaginatedResult<IdentityServer4.EntityFramework.Entities.Client>> Retrieve(ClientRequestModel model);
        Task<IdentityServer4.EntityFramework.Entities.Client> Single(ClientRequestModel model);
        Task<IdentityServer4.EntityFramework.Entities.Client> Add(ClientRequestModel model);
        Task<IdentityServer4.EntityFramework.Entities.Client> Modify(ClientRequestModel model);
        Task<IdentityServer4.EntityFramework.Entities.Client> Delete(ClientRequestModel model);
        Task<bool> UniqueClientId(int id, string clientId);
    }
}
