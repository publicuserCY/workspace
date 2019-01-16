using Demo4DotNetCore.AuthorizationServer.Model;
using Demo4DotNetCore.AuthorizationServer.RequestModel;
using System.Threading.Tasks;

namespace Demo4DotNetCore.AuthorizationServer.Service
{
    public interface IApiResourceService
    {
        Task<PaginatedResult<IdentityServer4.EntityFramework.Entities.ApiResource>> Retrieve(ApiResourceRequestModel model);
        Task<IdentityServer4.EntityFramework.Entities.ApiResource> Single(ApiResourceRequestModel model);
        Task<IdentityServer4.EntityFramework.Entities.ApiResource> Add(ApiResourceRequestModel model);
        Task<IdentityServer4.EntityFramework.Entities.ApiResource> Modify(ApiResourceRequestModel model);
        Task<IdentityServer4.EntityFramework.Entities.ApiResource> Delete(ApiResourceRequestModel model);
        Task<bool> UniqueApiResourceName(int id, string name);
    }
}
