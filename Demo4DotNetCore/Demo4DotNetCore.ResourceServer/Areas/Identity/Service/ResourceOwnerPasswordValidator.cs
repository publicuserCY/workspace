using Demo4DotNetCore.ResourceServer.Identity.Model;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Demo4DotNetCore.ResourceServer.Identity.Service
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private UserManager<ApplicationUser> UserManager { get; }

        public ResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = UserManager.FindByNameAsync(context.UserName).Result;
            if (user == null) { context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "用户名或密码错误"); }
            var result = UserManager.CheckPasswordAsync(user, context.Password).Result;
            if (!result) { context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "用户名或密码错误"); }
            var claims = new System.Security.Claims.Claim[]
            {
                new System.Security.Claims.Claim("UserName", user.UserName)
            };
            context.Result = new GrantValidationResult(user.UserName, IdentityModel.OidcConstants.AuthenticationMethods.Password, claims);
            context.Request.UserName = user.UserName;
            return Task.CompletedTask;
        }
    }
}
