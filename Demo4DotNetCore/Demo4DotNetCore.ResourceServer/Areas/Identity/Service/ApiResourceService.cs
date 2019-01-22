using Demo4DotNetCore.ResourceServer.Identity.RequestModel;
using Demo4DotNetCore.ResourceServer.Model;
using IdentityServer4.EntityFramework.DbContexts;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Demo4DotNetCore.ResourceServer.Identity.Service
{
    public class ApiResourceService : IApiResourceService
    {
        private ConfigurationDbContext DbContext { get; }

        public ApiResourceService(ConfigurationDbContext configurationDbContext)
        {
            DbContext = configurationDbContext;
        }

        public Task<PaginatedResult<IdentityServer4.EntityFramework.Entities.ApiResource>> Retrieve(ApiResourceRequestModel model)
        {
            var query = DbContext.ApiResources.AsQueryable();
            var predicate = PredicateBuilder.New<IdentityServer4.EntityFramework.Entities.ApiResource>();
            if (model.Enabled.HasValue)
            {
                predicate = predicate.And(p => p.Enabled == model.Enabled.Value);
                query = query.AsExpandable().Where(predicate);
            }
            if (!string.IsNullOrEmpty(model.Name))
            {
                predicate = predicate.And(p => p.Name.Contains(model.Name, StringComparison.OrdinalIgnoreCase));
                query = query.AsExpandable().Where(predicate);
            }
            if (!string.IsNullOrEmpty(model.DisplayName))
            {
                predicate = predicate.And(p => !string.IsNullOrEmpty(p.DisplayName) && p.DisplayName.Contains(model.DisplayName, StringComparison.OrdinalIgnoreCase));
                query = query.AsExpandable().Where(predicate);
            }
            if (!string.IsNullOrEmpty(model.Description))
            {
                predicate = predicate.And(p => !string.IsNullOrEmpty(p.Description) && p.Description.Contains(model.Description, StringComparison.OrdinalIgnoreCase));
                query = query.AsExpandable().Where(predicate);
            }
            var result = query.SortBy(model.SortExpression).ToPaginatedList(model.PageIndex, model.PageSize);
            return Task.FromResult(result);
        }

        public Task<IdentityServer4.EntityFramework.Entities.ApiResource> Single(ApiResourceRequestModel model)
        {
            var result = DbContext.ApiResources
                    .Include(p => p.Secrets)
                    .Include(p => p.Scopes).ThenInclude(scope => scope.UserClaims)
                    .Include(p => p.UserClaims)
                    .Include(p => p.Properties)
                    .SingleOrDefault(p => p.Id == int.Parse(model.Criteria));
            return Task.FromResult(result);
        }

        public Task<IdentityServer4.EntityFramework.Entities.ApiResource> Add(ApiResourceRequestModel model)
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
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
                    var entryApiResource = DbContext.Entry(apiResource);
                    entryApiResource.State = EntityState.Added;
                    DbContext.SaveChanges();
                    //entryApiResource.Reload();
                    /***** ApiSecret *****/
                    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiSecret> entryApiSecret = null;
                    IdentityServer4.EntityFramework.Entities.ApiSecret apiSecret = null;
                    model.ApiResource.Secrets.ForEach(item =>
                    {
                        switch (item.State)
                        {
                            case EntityState.Added:
                                apiSecret = new IdentityServer4.EntityFramework.Entities.ApiSecret()
                                {
                                    Description = item.Description,
                                    Value = item.Value,
                                    Expiration = item.Expiration ?? null,
                                    Type = item.Type,
                                    Created = DateTime.Now,
                                    ApiResourceId = apiResource.Id
                                };
                                entryApiSecret = DbContext.Entry(apiSecret);
                                entryApiSecret.State = EntityState.Added;
                                break;
                            default:
                                break;
                        }
                    });
                    DbContext.SaveChanges();
                    /***** ApiScope *****/
                    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiScope> entryApiScope = null;
                    IdentityServer4.EntityFramework.Entities.ApiScope apiScope = null;
                    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiScopeClaim> entryApiScopeClaim = null;
                    IdentityServer4.EntityFramework.Entities.ApiScopeClaim apiScopeClaim = null;
                    model.ApiResource.Scopes.ForEach(item =>
                    {
                        switch (item.State)
                        {
                            case EntityState.Added:
                                apiScope = new IdentityServer4.EntityFramework.Entities.ApiScope()
                                {
                                    Name = item.Name,
                                    DisplayName = item.DisplayName,
                                    Description = item.Description,
                                    Required = item.Required,
                                    Emphasize = item.Emphasize,
                                    ShowInDiscoveryDocument = item.ShowInDiscoveryDocument,
                                    ApiResourceId = apiResource.Id
                                };
                                entryApiScope = DbContext.Entry(apiScope);
                                entryApiScope.State = EntityState.Added;
                                break;
                            default:
                                break;
                        }
                        DbContext.SaveChanges();

                        item.UserClaims.ForEach(claim =>
                        {
                            switch (claim.State)
                            {
                                case EntityState.Added:
                                    apiScopeClaim = new IdentityServer4.EntityFramework.Entities.ApiScopeClaim()
                                    {
                                        Type = claim.Type,
                                        ApiScopeId = apiScope.Id
                                    };
                                    entryApiScopeClaim = DbContext.Entry(apiScopeClaim);
                                    entryApiScopeClaim.State = EntityState.Added;
                                    break;
                                default:
                                    break;
                            }
                        });
                    });
                    DbContext.SaveChanges();
                    /***** ApiResourceClaim *****/
                    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiResourceClaim> entryApiResourceClaim = null;
                    IdentityServer4.EntityFramework.Entities.ApiResourceClaim apiResourceClaim = null;
                    model.ApiResource.UserClaims.ForEach(item =>
                    {
                        switch (item.State)
                        {
                            case EntityState.Added:
                                apiResourceClaim = new IdentityServer4.EntityFramework.Entities.ApiResourceClaim()
                                {
                                    Type = item.Type,
                                    ApiResourceId = apiResource.Id
                                };
                                entryApiResourceClaim = DbContext.Entry(apiResourceClaim);
                                entryApiResourceClaim.State = EntityState.Added;
                                break;
                            default:
                                break;
                        }
                    });
                    DbContext.SaveChanges();
                    /***** ApiResourceProperty *****/
                    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiResourceProperty> entryApiResourceProperty = null;
                    IdentityServer4.EntityFramework.Entities.ApiResourceProperty apiResourceProperty = null;
                    model.ApiResource.Properties.ForEach(item =>
                    {
                        switch (item.State)
                        {
                            case EntityState.Added:
                                apiResourceProperty = new IdentityServer4.EntityFramework.Entities.ApiResourceProperty()
                                {
                                    Key = item.Key,
                                    Value = item.Value,
                                    ApiResourceId = apiResource.Id
                                };
                                entryApiResourceProperty = DbContext.Entry(apiResourceProperty);
                                entryApiResourceProperty.State = EntityState.Added;
                                break;
                            default:
                                break;
                        }
                    });
                    DbContext.SaveChanges();
                    transaction.Commit();
                    return Task.FromResult(entryApiResource.Entity);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            //var apiResource = new IdentityServer4.EntityFramework.Entities.ApiResource()
            //{
            //    Enabled = model.ApiResource.Enabled,
            //    Name = model.ApiResource.Name,
            //    DisplayName = model.ApiResource.DisplayName,
            //    Description = model.ApiResource.Description,
            //    Created = DateTime.Now,
            //    NonEditable = model.ApiResource.NonEditable
            //};

            //var entry = DbContext.Entry(apiResource);
            //entry.State = EntityState.Added;
            //DbContext.SaveChanges();
            //entry.Reload();
            //entry.Collection(p => p.Secrets).Load();
            //entry.Collection(p => p.Scopes).Load();
            //entry.Collection(p => p.UserClaims).Load();
            //entry.Collection(p => p.Properties).Load();
            //return Task.FromResult(entry.Entity);
        }

        public Task<IdentityServer4.EntityFramework.Entities.ApiResource> Modify(ApiResourceRequestModel model)
        {
            var apiResource = DbContext.ApiResources.SingleOrDefault(p => p.Id == model.ApiResource.Id);
            if (apiResource == null)
            {
                throw new Exception($"Id={model.ApiResource.Id}的ApiResource不存在");
            }
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    apiResource.Enabled = model.ApiResource.Enabled;
                    apiResource.Name = model.ApiResource.Name;
                    apiResource.DisplayName = model.ApiResource.DisplayName;
                    apiResource.Description = model.ApiResource.Description;
                    apiResource.Updated = DateTime.Now;
                    apiResource.NonEditable = model.ApiResource.NonEditable;

                    var entry = DbContext.Entry(apiResource);
                    entry.State = EntityState.Modified;
                    DbContext.SaveChanges();
                    /***** ApiSecret *****/
                    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiSecret> entryApiSecret = null;
                    IdentityServer4.EntityFramework.Entities.ApiSecret apiSecret = null;
                    model.ApiResource.Secrets.ForEach(item =>
                    {
                        switch (item.State)
                        {
                            case EntityState.Detached:
                                break;
                            case EntityState.Unchanged:
                                break;
                            case EntityState.Deleted:
                                apiSecret = new IdentityServer4.EntityFramework.Entities.ApiSecret() { Id = item.Id };
                                entryApiSecret = DbContext.Entry(apiSecret);
                                entryApiSecret.State = EntityState.Deleted;
                                break;
                            case EntityState.Modified:
                                apiSecret = new IdentityServer4.EntityFramework.Entities.ApiSecret()
                                {
                                    Id = item.Id,
                                    Description = item.Description,
                                    Value = item.Value,
                                    Expiration = item.Expiration ?? null,
                                    Type = item.Type
                                };
                                entryApiSecret = DbContext.Entry(apiSecret);
                                entryApiSecret.State = EntityState.Modified;
                                break;
                            case EntityState.Added:
                                apiSecret = new IdentityServer4.EntityFramework.Entities.ApiSecret()
                                {
                                    Description = item.Description,
                                    Value = item.Value,
                                    Expiration = item.Expiration ?? null,
                                    Type = item.Type,
                                    Created = DateTime.Now,
                                    ApiResourceId = apiResource.Id
                                };
                                entryApiSecret = DbContext.Entry(apiSecret);
                                entryApiSecret.State = EntityState.Added;
                                break;
                            default:
                                break;
                        }
                    });
                    DbContext.SaveChanges();
                    /***** ApiScope *****/
                    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiScope> entryApiScope = null;
                    IdentityServer4.EntityFramework.Entities.ApiScope apiScope = null;
                    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiScopeClaim> entryApiScopeClaim = null;
                    IdentityServer4.EntityFramework.Entities.ApiScopeClaim apiScopeClaim = null;
                    model.ApiResource.Scopes.ForEach(item =>
                    {
                        switch (item.State)
                        {
                            case EntityState.Detached:
                                break;
                            case EntityState.Unchanged:
                                break;
                            case EntityState.Deleted:
                                apiScope = new IdentityServer4.EntityFramework.Entities.ApiScope() { Id = item.Id };
                                entryApiScope = DbContext.Entry(apiScope);
                                entryApiScope.State = EntityState.Deleted;
                                break;
                            case EntityState.Modified:
                                apiScope = new IdentityServer4.EntityFramework.Entities.ApiScope()
                                {
                                    Id = item.Id,
                                    Name = item.Name,
                                    DisplayName = item.DisplayName,
                                    Description = item.Description,
                                    Required = item.Required,
                                    Emphasize = item.Emphasize,
                                    ShowInDiscoveryDocument = item.ShowInDiscoveryDocument
                                };
                                entryApiScope = DbContext.Entry(apiScope);
                                entryApiScope.State = EntityState.Modified;
                                break;
                            case EntityState.Added:
                                apiScope = new IdentityServer4.EntityFramework.Entities.ApiScope()
                                {
                                    Name = item.Name,
                                    DisplayName = item.DisplayName,
                                    Description = item.Description,
                                    Required = item.Required,
                                    Emphasize = item.Emphasize,
                                    ShowInDiscoveryDocument = item.ShowInDiscoveryDocument,
                                    ApiResourceId = apiResource.Id
                                };
                                entryApiScope = DbContext.Entry(apiScope);
                                entryApiScope.State = EntityState.Added;
                                break;
                            default:
                                break;
                        }
                        DbContext.SaveChanges();
                        item.UserClaims.ForEach(claim =>
                        {
                            switch (claim.State)
                            {
                                case EntityState.Detached:
                                    break;
                                case EntityState.Unchanged:
                                    break;
                                case EntityState.Deleted:
                                    apiScopeClaim = new IdentityServer4.EntityFramework.Entities.ApiScopeClaim() { Id = claim.Id };
                                    entryApiScopeClaim = DbContext.Entry(apiScopeClaim);
                                    entryApiScopeClaim.State = EntityState.Deleted;
                                    break;
                                case EntityState.Modified:
                                    apiScopeClaim = new IdentityServer4.EntityFramework.Entities.ApiScopeClaim()
                                    {
                                        Id = claim.Id,
                                        Type = claim.Type
                                    };
                                    entryApiScopeClaim = DbContext.Entry(apiScopeClaim);
                                    entryApiScopeClaim.State = EntityState.Modified;
                                    break;
                                case EntityState.Added:
                                    apiScopeClaim = new IdentityServer4.EntityFramework.Entities.ApiScopeClaim()
                                    {
                                        Type = claim.Type,
                                        ApiScopeId = item.Id
                                    };
                                    entryApiScopeClaim = DbContext.Entry(apiScopeClaim);
                                    entryApiScopeClaim.State = EntityState.Added;
                                    break;
                                default:
                                    break;
                            }
                        });
                        DbContext.SaveChanges();
                    });
                    /***** ApiResourceClaim *****/
                    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiResourceClaim> entryApiResourceClaim = null;
                    IdentityServer4.EntityFramework.Entities.ApiResourceClaim apiResourceClaim = null;
                    model.ApiResource.UserClaims.ForEach(item =>
                    {
                        switch (item.State)
                        {
                            case EntityState.Detached:
                                break;
                            case EntityState.Unchanged:
                                break;
                            case EntityState.Deleted:
                                apiResourceClaim = new IdentityServer4.EntityFramework.Entities.ApiResourceClaim() { Id = item.Id };
                                entryApiResourceClaim = DbContext.Entry(apiResourceClaim);
                                entryApiResourceClaim.State = EntityState.Deleted;
                                break;
                            case EntityState.Modified:
                                apiResourceClaim = new IdentityServer4.EntityFramework.Entities.ApiResourceClaim()
                                {
                                    Id = item.Id,
                                    Type = item.Type
                                };
                                entryApiResourceClaim = DbContext.Entry(apiResourceClaim);
                                entryApiResourceClaim.State = EntityState.Modified;
                                break;
                            case EntityState.Added:
                                apiResourceClaim = new IdentityServer4.EntityFramework.Entities.ApiResourceClaim()
                                {
                                    Type = item.Type,
                                    ApiResourceId = apiResource.Id
                                };
                                entryApiResourceClaim = DbContext.Entry(apiResourceClaim);
                                entryApiResourceClaim.State = EntityState.Added;
                                break;
                            default:
                                break;
                        }
                    });
                    DbContext.SaveChanges();
                    /***** ApiResourceProperty *****/
                    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiResourceProperty> entryApiResourceProperty = null;
                    IdentityServer4.EntityFramework.Entities.ApiResourceProperty apiResourceProperty = null;
                    model.ApiResource.Properties.ForEach(item =>
                    {
                        switch (item.State)
                        {
                            case EntityState.Detached:
                                break;
                            case EntityState.Unchanged:
                                break;
                            case EntityState.Deleted:
                                apiResourceProperty = new IdentityServer4.EntityFramework.Entities.ApiResourceProperty() { Id = item.Id };
                                entryApiResourceProperty = DbContext.Entry(apiResourceProperty);
                                entryApiResourceProperty.State = EntityState.Deleted;
                                break;
                            case EntityState.Modified:
                                apiResourceProperty = new IdentityServer4.EntityFramework.Entities.ApiResourceProperty()
                                {
                                    Id = item.Id,
                                    Key = item.Key,
                                    Value = item.Value
                                };
                                entryApiResourceProperty = DbContext.Entry(apiResourceProperty);
                                entryApiResourceProperty.State = EntityState.Modified;
                                break;
                            case EntityState.Added:
                                apiResourceProperty = new IdentityServer4.EntityFramework.Entities.ApiResourceProperty()
                                {
                                    Key = item.Key,
                                    Value = item.Value,
                                    ApiResourceId = apiResource.Id
                                };
                                entryApiResourceProperty = DbContext.Entry(apiResourceProperty);
                                entryApiResourceProperty.State = EntityState.Added;
                                break;
                            default:
                                break;
                        }
                    });
                    DbContext.SaveChanges();
                    entry.Reload();
                    transaction.Commit();
                    return Task.FromResult(entry.Entity);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public Task<IdentityServer4.EntityFramework.Entities.ApiResource> Delete(ApiResourceRequestModel model)
        {
            var apiResource = new IdentityServer4.EntityFramework.Entities.ApiResource() { Id = model.ApiResource.Id };
            var entry = DbContext.Entry(apiResource);
            entry.State = EntityState.Deleted;
            DbContext.SaveChanges();
            return Task.FromResult(entry.Entity);
        }

        public Task<bool> UniqueApiResourceName(int id, string name)
        {
            var result = DbContext.ApiResources.Any(p => !p.Id.Equals(id) && p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(result);
        }

        public Task<bool> UniqueApiScopeName(int id, string name)
        {
            var result = DbContext.ApiResources.SelectMany(apiResource => apiResource.Scopes).Any(p => !p.Id.Equals(id) && p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(result);
        }
    }
}