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
                    var entryApiResource = AddApiResource(model.ApiResource);
                    /***** ApiSecret *****/
                    model.ApiResource.Secrets.ForEach(item =>
                    {
                        switch (item.State)
                        {
                            case EntityState.Added:
                                AddApiSecret(model.ApiResource, item);
                                break;
                            default:
                                break;
                        }
                    });
                    /***** ApiScope *****/
                    model.ApiResource.Scopes.ForEach(item =>
                    {
                        switch (item.State)
                        {
                            case EntityState.Added:
                                AddApiScope(model.ApiResource, item);
                                break;
                            default:
                                break;
                        }
                        item.UserClaims.ForEach(claim =>
                        {
                            switch (claim.State)
                            {
                                case EntityState.Added:
                                    AddApiScopeClaim(item, claim);
                                    break;
                                default:
                                    break;
                            }
                        });
                    });
                    /***** ApiResourceClaim *****/
                    model.ApiResource.UserClaims.ForEach(item =>
                    {
                        switch (item.State)
                        {
                            case EntityState.Added:
                                AddApiResourceClaim(model.ApiResource, item);
                                break;
                            default:
                                break;
                        }
                    });
                    /***** ApiResourceProperty *****/
                    model.ApiResource.Properties.ForEach(item =>
                    {
                        switch (item.State)
                        {
                            case EntityState.Added:
                                AddApiResourceProperty(model.ApiResource, item);
                                break;
                            default:
                                break;
                        }
                    });
                    transaction.Commit();
                    return Task.FromResult(entryApiResource.Entity);
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            //entry.Reload();
            //entry.Collection(p => p.Secrets).Load();
            //entry.Collection(p => p.Scopes).Load();
            //entry.Collection(p => p.UserClaims).Load();
            //entry.Collection(p => p.Properties).Load();
            //return Task.FromResult(entry.Entity);
        }

        public Task<IdentityServer4.EntityFramework.Entities.ApiResource> Modify(ApiResourceRequestModel model)
        {
            if (!DbContext.ApiResources.Any(p => p.Id == model.ApiResource.Id))
            {
                throw new Exception($"Id={model.ApiResource.Id}的ApiResource不存在");
            }
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiResource> entryApiResource = null;
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    switch (model.ApiResource.State)
                    {
                        case EntityState.Modified:
                            entryApiResource = ModifyApiResource(model.ApiResource);
                            break;
                        default:
                            break;
                    }
                    if (entryApiResource == null) { entryApiResource = DbContext.Entry(new IdentityServer4.EntityFramework.Entities.ApiResource() { Id = model.ApiResource.Id }); }
                    /***** ApiSecret *****/
                    model.ApiResource.Secrets.ForEach(item =>
                    {
                        switch (item.State)
                        {
                            case EntityState.Detached:
                                break;
                            case EntityState.Unchanged:
                                break;
                            case EntityState.Deleted:
                                DeleteApiSecret(item);
                                break;
                            case EntityState.Modified:
                                ModifyApiSecret(model.ApiResource, item);
                                break;
                            case EntityState.Added:
                                AddApiSecret(model.ApiResource, item);
                                break;
                            default:
                                break;
                        }
                    });
                    /***** ApiScope *****/
                    model.ApiResource.Scopes.ForEach(item =>
                    {
                        switch (item.State)
                        {
                            case EntityState.Detached:
                                break;
                            case EntityState.Unchanged:
                                break;
                            case EntityState.Deleted:
                                DeleteApiScope(item);
                                break;
                            case EntityState.Modified:
                                ModifyApiScope(model.ApiResource, item);
                                break;
                            case EntityState.Added:
                                AddApiScope(model.ApiResource, item);
                                break;
                            default:
                                break;
                        }

                        if (item.State != EntityState.Deleted)
                        {
                            item.UserClaims.ForEach(claim =>
                            {
                                switch (claim.State)
                                {
                                    case EntityState.Detached:
                                        break;
                                    case EntityState.Unchanged:
                                        break;
                                    case EntityState.Deleted:
                                        DeleteApiScopeClaim(claim);
                                        break;
                                    case EntityState.Modified:
                                        ModifyApiScopeClaim(item, claim);
                                        break;
                                    case EntityState.Added:
                                        AddApiScopeClaim(item, claim);
                                        break;
                                    default:
                                        break;
                                }
                            });
                        }
                    });
                    /***** ApiResourceClaim *****/
                    model.ApiResource.UserClaims.ForEach(item =>
                    {
                        switch (item.State)
                        {
                            case EntityState.Detached:
                                break;
                            case EntityState.Unchanged:
                                break;
                            case EntityState.Deleted:
                                DeleteApiResourceClaim(item);
                                break;
                            case EntityState.Modified:
                                ModifyApiResourceClaim(model.ApiResource, item);
                                break;
                            case EntityState.Added:
                                AddApiResourceClaim(model.ApiResource, item);
                                break;
                            default:
                                break;
                        }
                    });
                    /***** ApiResourceProperty *****/
                    model.ApiResource.Properties.ForEach(item =>
                    {
                        switch (item.State)
                        {
                            case EntityState.Detached:
                                break;
                            case EntityState.Unchanged:
                                break;
                            case EntityState.Deleted:
                                DeleteApiResourceProperty(item);
                                break;
                            case EntityState.Modified:
                                ModifyApiResourceProperty(model.ApiResource, item);
                                break;
                            case EntityState.Added:
                                AddApiResourceProperty(model.ApiResource, item);
                                break;
                            default:
                                break;
                        }
                        DbContext.SaveChanges();
                    });
                    entryApiResource.Reload();
                    transaction.Commit();
                    return Task.FromResult(entryApiResource.Entity);
                }
                catch
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

        #region ApiResource
        private Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiResource> AddApiResource(Model.ApiResource model)
        {
            var apiResource = new IdentityServer4.EntityFramework.Entities.ApiResource()
            {
                Enabled = model.Enabled,
                Name = model.Name,
                DisplayName = model.DisplayName,
                Description = model.Description,
                Created = DateTime.Now,
                NonEditable = model.NonEditable
            };
            var entry = DbContext.Entry(apiResource);
            entry.State = EntityState.Added;
            DbContext.SaveChanges();
            return entry;
        }

        private Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiResource> ModifyApiResource(Model.ApiResource model)
        {
            var apiResource = new IdentityServer4.EntityFramework.Entities.ApiResource()
            {
                Id = model.Id,
                Enabled = model.Enabled,
                Name = model.Name,
                DisplayName = model.DisplayName,
                Description = model.Description,
                Created = model.Created,
                Updated = DateTime.Now,
                NonEditable = model.NonEditable
            };
            var entry = DbContext.Entry(apiResource);
            entry.State = EntityState.Modified;
            DbContext.SaveChanges();
            return entry;
        }

        private Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiResource> DeleteApiResource(Model.ApiResource model)
        {
            var apiResource = new IdentityServer4.EntityFramework.Entities.ApiResource() { Id = model.Id };
            var entry = DbContext.Entry(apiResource);
            entry.State = EntityState.Deleted;
            DbContext.SaveChanges();
            return entry;
        }
        #endregion

        #region ApiSecret
        private Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiSecret> AddApiSecret(Model.ApiResource apiResource, Model.ApiSecret model)
        {
            var apiSecret = new IdentityServer4.EntityFramework.Entities.ApiSecret()
            {
                Description = model.Description,
                Value = model.Value,
                Expiration = model.Expiration ?? null,
                Type = model.Type,
                Created = DateTime.Now,
                ApiResourceId = apiResource.Id
            };
            var entry = DbContext.Entry(apiSecret);
            entry.State = EntityState.Added;
            DbContext.SaveChanges();
            return entry;
        }

        private Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiSecret> ModifyApiSecret(Model.ApiResource apiResource, Model.ApiSecret model)
        {
            var apiSecret = new IdentityServer4.EntityFramework.Entities.ApiSecret()
            {
                Id = model.Id,
                Description = model.Description,
                Value = model.Value,
                Expiration = model.Expiration ?? null,
                Type = model.Type,
                Created = model.Created,
                ApiResourceId = apiResource.Id
            };
            var entry = DbContext.Entry(apiSecret);
            entry.State = EntityState.Modified;
            DbContext.SaveChanges();
            return entry;
        }

        private Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiSecret> DeleteApiSecret(Model.ApiSecret model)
        {
            var apiSecret = new IdentityServer4.EntityFramework.Entities.ApiSecret() { Id = model.Id };
            var entry = DbContext.Entry(apiSecret);
            entry.State = EntityState.Deleted;
            DbContext.SaveChanges();
            return entry;
        }
        #endregion

        #region ApiScope
        private Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiScope> AddApiScope(Model.ApiResource apiResource, Model.ApiScope model)
        {
            var apiScope = new IdentityServer4.EntityFramework.Entities.ApiScope()
            {
                Name = model.Name,
                DisplayName = model.DisplayName,
                Description = model.Description,
                Required = model.Required,
                Emphasize = model.Emphasize,
                ShowInDiscoveryDocument = model.ShowInDiscoveryDocument,
                ApiResourceId = apiResource.Id
            };
            var entry = DbContext.Entry(apiScope);
            entry.State = EntityState.Added;
            DbContext.SaveChanges();
            model.Id = apiScope.Id;
            return entry;
        }

        private Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiScope> ModifyApiScope(Model.ApiResource apiResource, Model.ApiScope model)
        {
            var apiScope = new IdentityServer4.EntityFramework.Entities.ApiScope()
            {
                Id = model.Id,
                Name = model.Name,
                DisplayName = model.DisplayName,
                Description = model.Description,
                Required = model.Required,
                Emphasize = model.Emphasize,
                ShowInDiscoveryDocument = model.ShowInDiscoveryDocument,
                ApiResourceId = apiResource.Id
            };
            var entry = DbContext.Entry(apiScope);
            entry.State = EntityState.Modified;
            DbContext.SaveChanges();
            model.Id = apiScope.Id;
            return entry;
        }

        private Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiScope> DeleteApiScope(Model.ApiScope model)
        {
            var apiScope = new IdentityServer4.EntityFramework.Entities.ApiScope() { Id = model.Id };
            var entry = DbContext.Entry(apiScope);
            entry.State = EntityState.Deleted;
            DbContext.SaveChanges();
            return entry;
        }
        #endregion

        #region ApiScopeClaim
        private Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiScopeClaim> AddApiScopeClaim(Model.ApiScope apiScope, Model.ApiScopeClaim model)
        {
            var apiScopeClaim = new IdentityServer4.EntityFramework.Entities.ApiScopeClaim()
            {
                Type = model.Type,
                ApiScopeId = apiScope.Id
            };
            var entry = DbContext.Entry(apiScopeClaim);
            entry.State = EntityState.Added;
            DbContext.SaveChanges();
            return entry;
        }

        private Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiScopeClaim> ModifyApiScopeClaim(Model.ApiScope apiScope, Model.ApiScopeClaim model)
        {
            var apiScopeClaim = new IdentityServer4.EntityFramework.Entities.ApiScopeClaim()
            {
                Id = model.Id,
                Type = model.Type,
                ApiScopeId = apiScope.Id
            };
            var entry = DbContext.Entry(apiScopeClaim);
            entry.State = EntityState.Modified;
            DbContext.SaveChanges();
            return entry;
        }

        private Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiScopeClaim> DeleteApiScopeClaim(Model.ApiScopeClaim model)
        {
            var apiScopeClaim = new IdentityServer4.EntityFramework.Entities.ApiScopeClaim() { Id = model.Id };
            var entry = DbContext.Entry(apiScopeClaim);
            entry.State = EntityState.Deleted;
            DbContext.SaveChanges();
            return entry;
        }
        #endregion

        #region ApiResourceClaim
        private Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiResourceClaim> AddApiResourceClaim(Model.ApiResource apiResource, Model.ApiResourceClaim model)
        {
            var apiResourceClaim = new IdentityServer4.EntityFramework.Entities.ApiResourceClaim()
            {
                Type = model.Type,
                ApiResourceId = apiResource.Id
            };
            var entry = DbContext.Entry(apiResourceClaim);
            entry.State = EntityState.Added;
            DbContext.SaveChanges();
            return entry;
        }

        private Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiResourceClaim> ModifyApiResourceClaim(Model.ApiResource apiResource, Model.ApiResourceClaim model)
        {
            var apiResourceClaim = new IdentityServer4.EntityFramework.Entities.ApiResourceClaim()
            {
                Id = model.Id,
                Type = model.Type,
                ApiResourceId = apiResource.Id
            };
            var entry = DbContext.Entry(apiResourceClaim);
            entry.State = EntityState.Modified;
            DbContext.SaveChanges();
            return entry;
        }

        private Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiResourceClaim> DeleteApiResourceClaim(Model.ApiResourceClaim model)
        {
            var apiResourceClaim = new IdentityServer4.EntityFramework.Entities.ApiResourceClaim() { Id = model.Id };
            var entry = DbContext.Entry(apiResourceClaim);
            entry.State = EntityState.Deleted;
            DbContext.SaveChanges();
            return entry;
        }
        #endregion

        #region ApiResourceProperty
        private Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiResourceProperty> AddApiResourceProperty(Model.ApiResource apiResource, Model.ApiResourceProperty model)
        {
            var apiResourceProperty = new IdentityServer4.EntityFramework.Entities.ApiResourceProperty()
            {
                Key = model.Key,
                Value = model.Value,
                ApiResourceId = apiResource.Id
            };
            var entry = DbContext.Entry(apiResourceProperty);
            entry.State = EntityState.Added;
            DbContext.SaveChanges();
            return entry;
        }

        private Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiResourceProperty> ModifyApiResourceProperty(Model.ApiResource apiResource, Model.ApiResourceProperty model)
        {
            var apiResourceProperty = new IdentityServer4.EntityFramework.Entities.ApiResourceProperty()
            {
                Id = model.Id,
                Key = model.Key,
                Value = model.Value,
                ApiResourceId = apiResource.Id
            };
            var entry = DbContext.Entry(apiResourceProperty);
            entry.State = EntityState.Modified;
            DbContext.SaveChanges();
            return entry;
        }

        private Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityServer4.EntityFramework.Entities.ApiResourceProperty> DeleteApiResourceProperty(Model.ApiResourceProperty model)
        {
            var apiResourceProperty = new IdentityServer4.EntityFramework.Entities.ApiResourceProperty() { Id = model.Id };
            var entry = DbContext.Entry(apiResourceProperty);
            entry.State = EntityState.Deleted;
            DbContext.SaveChanges();
            return entry;
        }
        #endregion

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