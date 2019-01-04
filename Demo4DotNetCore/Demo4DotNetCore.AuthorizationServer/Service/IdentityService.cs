using Demo4DotNetCore.AuthorizationServer.Model;
using Demo4DotNetCore.AuthorizationServer.RequestModel;
using Demo4DotNetCore.Tools;
using IdentityServer4.EntityFramework.DbContexts;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Demo4DotNetCore.AuthorizationServer.Service
{
    public class IdentityService : IIdentityService
    {
        private UserManager<ApplicationUser> UserManager { get; }
        private ConfigurationDbContext ConfigurationDbContext { get; }

        public IdentityService(UserManager<ApplicationUser> userManager, ConfigurationDbContext configurationDbContext)
        {
            UserManager = userManager;
            ConfigurationDbContext = configurationDbContext;
        }

        #region ApiResource
        public Task<PaginatedList<IdentityServer4.EntityFramework.Entities.ApiResource>> SelectApiResource(ApiResourceRequestModel model)
        {
            var query = ConfigurationDbContext.ApiResources
                    .Include(p => p.Secrets)
                    .Include(p => p.Scopes)
                    .Include(p => p.UserClaims)
                    .Include(p => p.Properties)
                    .AsQueryable();
            var predicate = PredicateBuilder.New<IdentityServer4.EntityFramework.Entities.ApiResource>();
            if (model.Id != 0)
            {
                predicate = predicate.And(p => p.Id == model.Id);
                query = query.AsExpandable().Where(predicate);
            }
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                predicate = predicate.And(p => p.Name.Contains(model.Name, StringComparison.OrdinalIgnoreCase));
                query = query.AsExpandable().Where(predicate);
            }
            if (!string.IsNullOrWhiteSpace(model.Description))
            {
                predicate = predicate.And(p => p.Description.Contains(model.Description, StringComparison.OrdinalIgnoreCase));
                query = query.AsExpandable().Where(predicate);
            }
            var result = query.SortBy(model.SortExpression).ToPaginatedList(model.PageIndex, model.PageSize);
            return Task.FromResult(result);
        }

        public Task<IdentityServer4.EntityFramework.Entities.ApiResource> InsertApiResource(ApiResourceRequestModel model)
        {
            var apiResource = new IdentityServer4.EntityFramework.Entities.ApiResource()
            {
                Enabled = model.ApiResource.Enabled,
                Name = model.ApiResource.Name,
                DisplayName = model.ApiResource.DisplayName,
                Description = model.ApiResource.Description,
                Created = DateTime.Now,
                NonEditable = model.ApiResource.NonEditable
            };

            //Secrets
            model.ApiResource.Secrets.ForEach(p =>
            {
                if (p.State == EntityState.Added)
                {
                    var secret = new IdentityServer4.EntityFramework.Entities.ApiSecret()
                    {
                        ApiResourceId = apiResource.Id,
                        Description = p.Description,
                        Value = p.Value,
                        Expiration = p.Expiration,
                        Type = p.Type,
                        Created = p.Created
                    };
                    ConfigurationDbContext.Entry(secret).State = EntityState.Added;
                };
            });
            var entry = ConfigurationDbContext.Entry(apiResource);
            entry.State = EntityState.Added; ;
            ConfigurationDbContext.SaveChanges();
            entry.Reload();
            return Task.FromResult(entry.Entity);
        }

        public Task<IdentityServer4.EntityFramework.Entities.ApiResource> UpdateApiResource(ApiResourceRequestModel model)
        {
            var apiResource = ConfigurationDbContext.ApiResources.SingleOrDefault(p => p.Id == model.Id);
            if (apiResource == null)
            {
                throw new Exception($"Id={model.Id}的Api Resource 不存在");
            }
            apiResource.Enabled = model.ApiResource.Enabled;
            apiResource.Name = model.ApiResource.Name;
            apiResource.DisplayName = model.ApiResource.DisplayName;
            apiResource.Description = model.ApiResource.Description;
            apiResource.Updated = DateTime.Now;
            apiResource.NonEditable = model.ApiResource.NonEditable;
            ConfigurationDbContext.Entry(apiResource).State = EntityState.Modified;

            //Secrets
            model.ApiResource.Secrets.ForEach(p =>
            {
                if (p.State == EntityState.Added)
                {
                    var secret = new IdentityServer4.EntityFramework.Entities.ApiSecret()
                    {
                        Description = p.Description,
                        Value = p.Value,
                        Expiration = p.Expiration,
                        Type = p.Type,
                        Created = p.Created,
                        ApiResourceId = apiResource.Id
                    };
                    ConfigurationDbContext.Entry(secret).State = EntityState.Added;
                }
                else if (p.State == EntityState.Modified)
                {
                    var secret = new IdentityServer4.EntityFramework.Entities.ApiSecret()
                    {
                        Id = p.Id,
                        Description = p.Description,
                        Value = p.Value,
                        Expiration = p.Expiration,
                        Type = p.Type,
                        Created = p.Created,
                        ApiResourceId = apiResource.Id,
                    };
                    ConfigurationDbContext.Entry(secret).State = EntityState.Modified;
                }
                else if (p.State == EntityState.Deleted)
                {
                    var secret = new IdentityServer4.EntityFramework.Entities.ApiSecret() { Id = p.Id };
                    ConfigurationDbContext.Entry(secret).State = EntityState.Deleted;
                };
            });

            ConfigurationDbContext.SaveChanges();
            return Task.FromResult(apiResource);
        }

        public Task<IdentityServer4.EntityFramework.Entities.ApiResource> DeleteApiResource(ApiResourceRequestModel model)
        {
            var entity = ConfigurationDbContext.ApiResources.SingleOrDefault(p => p.Id == model.Id);
            if (entity == null)
            {
                throw new Exception("Api Resource 不存在");
            }
            ConfigurationDbContext.Entry(entity).State = EntityState.Deleted;
            ConfigurationDbContext.SaveChanges();
            return Task.FromResult(entity);
        }

        public Task<bool> UniqueApiResourceName(int id, string name)
        {
            var result = ConfigurationDbContext.ApiResources.Any(p => !p.Id.Equals(id) && p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(result);
        }
        #endregion



        //public Task<ApplicationUser> InsertAccount(AccountDto dto)
        //{
        //    var user = UserManager.FindByNameAsync(dto.UserName).Result;
        //    if (user != null)
        //    {
        //        throw new Exception("用户已存在");
        //    }
        //    user = new ApplicationUser()
        //    {
        //        UserName = dto.UserName
        //    };
        //    var result = UserManager.CreateAsync(user, dto.Password).Result;
        //    if (!result.Succeeded)
        //    {
        //        throw new Exception(result.Errors.First().Description);
        //    }
        //    user = UserManager.FindByNameAsync(dto.UserName).Result;
        //    return Task.FromResult(user);
        //}

        public ApplicationUser FindByExternalProvider(string provider, string userId)
        {
            throw new NotImplementedException();
        }

        public ApplicationUser FindBySubjectId(string subjectId)
        {
            return UserManager.FindByIdAsync(subjectId).Result;
        }

        public ApplicationUser FindByUsername(string username)
        {
            return UserManager.FindByNameAsync(username).Result;
        }

        public bool ValidateCredentials(string username, string password)
        {
            var user = UserManager.FindByNameAsync(username).Result;
            return UserManager.CheckPasswordAsync(user, password).Result;
        }

        public ApplicationUser AutoProvisionUser(string provider, string userId, List<Claim> claims)
        {
            throw new NotImplementedException();
        }
    }
}
