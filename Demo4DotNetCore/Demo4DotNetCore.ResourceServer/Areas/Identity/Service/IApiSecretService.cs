using Demo4DotNetCore.ResourceServer.Identity.RequestModel;
using System.Threading.Tasks;

namespace Demo4DotNetCore.ResourceServer.Identity.Service
{
    public interface IApiSecretService
    {
        Task<IdentityServer4.EntityFramework.Entities.ApiSecret> Single(ApiSecretRequestModel model);
        Task<IdentityServer4.EntityFramework.Entities.ApiSecret> Add(ApiSecretRequestModel model);
        Task<IdentityServer4.EntityFramework.Entities.ApiSecret> Modify(ApiSecretRequestModel model);
        Task<IdentityServer4.EntityFramework.Entities.ApiSecret> Delete(ApiSecretRequestModel model);
    }
}
