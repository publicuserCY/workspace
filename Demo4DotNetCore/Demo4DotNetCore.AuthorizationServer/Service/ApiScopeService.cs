using Demo4DotNetCore.AuthorizationServer.RequestModel;
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
    public class ApiScopeService : IBaseService<ApiScopeRequestModel, IdentityServer4.EntityFramework.Entities.ApiScope>
    {
        private ConfigurationDbContext ConfigurationDbContext { get; }

        public ApiScopeService(ConfigurationDbContext configurationDbContext)
        {
            ConfigurationDbContext = configurationDbContext;
        }

        public Task<IdentityServer4.EntityFramework.Entities.ApiScope> Single(ApiScopeRequestModel model)
        {
            var query = ConfigurationDbContext.ApiResources
                     .Include(p => p.Scopes)
                     .ThenInclude(scope => scope.UserClaims)
                     .SelectMany(apiResource => apiResource.Scopes)
                     .AsQueryable();
            var predicate = PredicateBuilder.New<IdentityServer4.EntityFramework.Entities.ApiScope>();
            if (model.Id.HasValue)
            {
                predicate = predicate.And(p => p.Id == model.Id.Value);
                query = query.AsExpandable().Where(predicate);
            }
            var result = query.SingleOrDefault();
            return Task.FromResult(result);
        }

        public Task<IdentityServer4.EntityFramework.Entities.ApiScope> Add(ApiScopeRequestModel model)
        {
            var apiScope = new IdentityServer4.EntityFramework.Entities.ApiScope()
            {
                Name = model.ApiScope.Name,
                DisplayName = model.ApiScope.DisplayName,
                Description = model.ApiScope.Description,
                Required = model.ApiScope.Required,
                Emphasize = model.ApiScope.Emphasize,
                ShowInDiscoveryDocument = model.ApiScope.ShowInDiscoveryDocument,
                ApiResourceId = model.ApiScope.ApiResourceId
            };
            var entry = ConfigurationDbContext.Entry(apiScope);
            entry.State = EntityState.Added;
            ConfigurationDbContext.SaveChanges();
            entry.Reload();
            return Task.FromResult(entry.Entity);
        }

        public Task<IdentityServer4.EntityFramework.Entities.ApiScope> Modify(ApiScopeRequestModel model)
        {
            var apiScope = new IdentityServer4.EntityFramework.Entities.ApiScope()
            {
                Id = model.ApiScope.Id,
                Name = model.ApiScope.Name,
                DisplayName = model.ApiScope.DisplayName,
                Description = model.ApiScope.Description,
                Required = model.ApiScope.Required,
                Emphasize = model.ApiScope.Emphasize,
                ShowInDiscoveryDocument = model.ApiScope.ShowInDiscoveryDocument,
                ApiResourceId = model.ApiScope.ApiResourceId
            };
            var entry = ConfigurationDbContext.Entry(apiScope);
            entry.State = EntityState.Modified;
            ConfigurationDbContext.SaveChanges();
            entry.Reload();
            return Task.FromResult(entry.Entity);
        }

        public Task<IdentityServer4.EntityFramework.Entities.ApiScope> Delete(ApiScopeRequestModel model)
        {
            var apiScope = new IdentityServer4.EntityFramework.Entities.ApiScope() { Id = model.ApiScope.Id };
            var entry = ConfigurationDbContext.Entry(apiScope);
            entry.State = EntityState.Deleted;
            ConfigurationDbContext.SaveChanges();
            return Task.FromResult(apiScope);
        }
    }
}
