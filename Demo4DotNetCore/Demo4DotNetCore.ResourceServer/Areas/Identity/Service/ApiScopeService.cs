using Demo4DotNetCore.ResourceServer.Identity.RequestModel;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo4DotNetCore.ResourceServer.Identity.Service
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
            entry.Collection(p => p.UserClaims).Load();
            return Task.FromResult(entry.Entity);
        }

        public Task<IdentityServer4.EntityFramework.Entities.ApiScope> Modify(ApiScopeRequestModel model)
        {
            var apiScope = DbContext.ApiResources.SelectMany(p => p.Scopes).SingleOrDefault(p => p.Id == model.ApiScope.Id);
            if (apiScope == null)
            {
                throw new Exception($"Id={model.ApiScope.Id}的ApiScope不存在");
            }
            apiScope.Name = model.ApiScope.Name;
            apiScope.DisplayName = model.ApiScope.DisplayName;
            apiScope.Description = model.ApiScope.Description;
            apiScope.Required = model.ApiScope.Required;
            apiScope.Emphasize = model.ApiScope.Emphasize;
            apiScope.ShowInDiscoveryDocument = model.ApiScope.ShowInDiscoveryDocument;

            var entry = DbContext.Entry(apiScope);
            entry.State = EntityState.Modified;
            DbContext.SaveChanges();
            entry.Reload();
            entry.Collection(p => p.UserClaims).Load();
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

        public Task<bool> UniqueApiScopeName(int id, string name)
        {
            var result = DbContext.ApiResources.SelectMany(apiResource => apiResource.Scopes).Any(p => !p.Id.Equals(id) && p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(result);
        }
    }
}
