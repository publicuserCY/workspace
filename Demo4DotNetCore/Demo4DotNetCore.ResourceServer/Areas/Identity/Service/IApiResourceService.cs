using Demo4DotNetCore.ResourceServer.Identity.RequestModel;
using Demo4DotNetCore.ResourceServer.Model;
using System.Threading.Tasks;

namespace Demo4DotNetCore.ResourceServer.Identity.Service
{
    public interface IApiResourceService
    {
        Task<PaginatedResult<IdentityServer4.EntityFramework.Entities.ApiResource>> Retrieve(ApiResourceRequestModel model);
        Task<IdentityServer4.EntityFramework.Entities.ApiResource> Single(ApiResourceRequestModel model);
        Task<IdentityServer4.EntityFramework.Entities.ApiResource> Add(ApiResourceRequestModel model);
        Task<IdentityServer4.EntityFramework.Entities.ApiResource> Modify(ApiResourceRequestModel model);
        Task<IdentityServer4.EntityFramework.Entities.ApiResource> Delete(ApiResourceRequestModel model);
        Task<bool> UniqueApiResourceName(int id, string name);
        Task<bool> UniqueApiScopeName(int id, string name);
    }
}
