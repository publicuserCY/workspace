using Demo4DotNetCore.ResourceServer.Identity.RequestModel;
using System.Threading.Tasks;

namespace Demo4DotNetCore.ResourceServer.Identity.Service
{
    public interface IApiScopeService
    {
        Task<IdentityServer4.EntityFramework.Entities.ApiScope> Single(ApiScopeRequestModel model);
        Task<IdentityServer4.EntityFramework.Entities.ApiScope> Add(ApiScopeRequestModel model);
        Task<IdentityServer4.EntityFramework.Entities.ApiScope> Modify(ApiScopeRequestModel model);
        Task<IdentityServer4.EntityFramework.Entities.ApiScope> Delete(ApiScopeRequestModel model);
        Task<bool> UniqueApiScopeName(int id, string name);
    }
}
