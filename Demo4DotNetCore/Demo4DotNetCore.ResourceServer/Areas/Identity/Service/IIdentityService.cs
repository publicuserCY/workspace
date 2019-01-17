using Demo4DotNetCore.ResourceServer.Identity.Model;
using System.Collections.Generic;
using System.Security.Claims;

namespace Demo4DotNetCore.ResourceServer.Identity.Service
{
    public interface IIdentityService
    {
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
