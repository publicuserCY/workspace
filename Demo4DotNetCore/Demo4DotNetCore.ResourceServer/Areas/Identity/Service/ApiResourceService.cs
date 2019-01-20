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
            var apiResource = new IdentityServer4.EntityFramework.Entities.ApiResource()
            {
                Enabled = model.ApiResource.Enabled,
                Name = model.ApiResource.Name,
                DisplayName = model.ApiResource.DisplayName,
                Description = model.ApiResource.Description,
                Created = DateTime.Now,
                NonEditable = model.ApiResource.NonEditable
            };

            var entry = DbContext.Entry(apiResource);           
            entry.State = EntityState.Added;
            DbContext.SaveChanges();
            entry.Reload();
            entry.Collection(p => p.Secrets).Load();
            entry.Collection(p => p.Scopes).Load();
            entry.Collection(p => p.UserClaims).Load();
            entry.Collection(p => p.Properties).Load();
            return Task.FromResult(entry.Entity);
        }

        public Task<IdentityServer4.EntityFramework.Entities.ApiResource> Modify(ApiResourceRequestModel model)
        {
            var apiResource = DbContext.ApiResources.SingleOrDefault(p => p.Id == model.ApiResource.Id);
            if (apiResource == null)
            {
                throw new Exception($"Id={model.ApiResource.Id}的ApiResource不存在");
            }
            apiResource.Enabled = model.ApiResource.Enabled;
            apiResource.Name = model.ApiResource.Name;
            apiResource.DisplayName = model.ApiResource.DisplayName;
            apiResource.Description = model.ApiResource.Description;
            apiResource.Updated = DateTime.Now;
            apiResource.NonEditable = model.ApiResource.NonEditable;

            var entry = DbContext.Entry(apiResource);
            entry.State = EntityState.Modified;
            DbContext.SaveChanges();
            entry.Reload();
            entry.Collection(p => p.Secrets).Load();
            entry.Collection(p => p.Scopes).Load();
            entry.Collection(p => p.UserClaims).Load();
            entry.Collection(p => p.Properties).Load();
            return Task.FromResult(entry.Entity);
        }

        public Task<IdentityServer4.EntityFramework.Entities.ApiResource> Delete(ApiResourceRequestModel model)
        {
            //var entity = ConfigurationDbContext.ApiResources.SingleOrDefault(p => p.Id == model.Id);
            //if (entity == null)
            //{
            //    throw new Exception("Api Resource 不存在");
            //}
            //ConfigurationDbContext.Entry(entity).State = EntityState.Deleted;

            var apiResource = new IdentityServer4.EntityFramework.Entities.ApiResource()
            {
                Id = model.ApiResource.Id
            };

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
    }
}
