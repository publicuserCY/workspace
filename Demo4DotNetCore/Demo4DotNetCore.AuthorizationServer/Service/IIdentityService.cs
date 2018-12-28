using Demo4DotNetCore.AuthorizationServer.Dto;
using Demo4DotNetCore.AuthorizationServer.Model;
using IdentityServer4.EntityFramework.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Demo4DotNetCore.AuthorizationServer.Service
{
    public interface IIdentityService
    {
        Task<PaginatedList<ApiResource>> SelectApiResource(ApiResourceRequestModel model);
        Task<ApiResource> InsertApiResource(ApiResourceRequestModel model);
        Task<ApiResource> UpdateApiResource(ApiResourceRequestModel model);
        Task<ApiResource> DeleteApiResource(ApiResourceRequestModel model);

        ApplicationUser AutoProvisionUser(string provider, string userId, List<Claim> claims);
        ApplicationUser FindByExternalProvider(string provider, string userId);
        ApplicationUser FindBySubjectId(string subjectId);
        ApplicationUser FindByUsername(string username);
        bool ValidateCredentials(string username, string password);
        Task<ApplicationUser> InsertAccount(AccountDto dto);
        //Task<ApplicationUser> UpdateAccount(ApplicationUser model);
        //Task<ApplicationUser> DeleteAccount(ApplicationUser model);
    }
}
