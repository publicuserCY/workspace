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
    public class ApiScopeService : IApiScopeService
    {
        private ConfigurationDbContext DbContext { get; }

        public ApiScopeService(ConfigurationDbContext configurationDbContext)
        {
            DbContext = configurationDbContext;
        }

        public Task<IdentityServer4.EntityFramework.Entities.ApiScope> Single(ApiScopeRequestModel model)
        {
            var query = DbContext.ApiResources
                     .Include(p => p.Scopes)
                     .ThenInclude(scope => scope.UserClaims)
                     .SelectMany(apiResource => apiResource.Scopes);
            var result = query.SingleOrDefault(p => p.Id == int.Parse(model.Criteria));
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
            var entry = DbContext.Entry(apiScope);
            entry.State = EntityState.Added;
            DbContext.SaveChanges();
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
            var entry = DbContext.Entry(apiScope);
            entry.State = EntityState.Modified;
            DbContext.SaveChanges();
            entry.Reload();
            return Task.FromResult(entry.Entity);
        }

        public Task<IdentityServer4.EntityFramework.Entities.ApiScope> Delete(ApiScopeRequestModel model)
        {
            var apiScope = new IdentityServer4.EntityFramework.Entities.ApiScope() { Id = model.ApiScope.Id };
            var entry = DbContext.Entry(apiScope);
            entry.State = EntityState.Deleted;
            DbContext.SaveChanges();
            return Task.FromResult(apiScope);
        }
    }
}
