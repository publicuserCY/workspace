using Demo4DotNetCore.AuthorizationServer.Model;
using Demo4DotNetCore.AuthorizationServer.RequestModel;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Demo4DotNetCore.AuthorizationServer.Service
{
    public interface IIdentityService
    {
        Task<PaginatedList<IdentityServer4.EntityFramework.Entities.ApiResource>> SelectApiResource(ApiResourceRequestModel model);
        Task<IdentityServer4.EntityFramework.Entities.ApiResource> InsertApiResource(ApiResourceRequestModel model);
        Task<IdentityServer4.EntityFramework.Entities.ApiResource> UpdateApiResource(ApiResourceRequestModel model);
        Task<IdentityServer4.EntityFramework.Entities.ApiResource> DeleteApiResource(ApiResourceRequestModel model);
        Task<bool> UniqueApiResourceName(int id, string name);

        ApplicationUser AutoProvisionUser(string provider, string userId, List<Claim> claims);
        ApplicationUser FindByExternalProvider(string provider, string userId);
        ApplicationUser FindBySubjectId(string subjectId);
        ApplicationUser FindByUsername(string username);
        bool ValidateCredentials(string username, string password);
        //Task<ApplicationUser> InsertAccount(AccountDto dto);
        //Task<ApplicationUser> UpdateAccount(ApplicationUser model);
        //Task<ApplicationUser> DeleteAccount(ApplicationUser model);
    }
}
