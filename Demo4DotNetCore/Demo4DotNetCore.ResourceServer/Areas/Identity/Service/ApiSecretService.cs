using Demo4DotNetCore.ResourceServer.Identity.RequestModel;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo4DotNetCore.ResourceServer.Identity.Service
{
    public class ApiSecretService : IApiSecretService
    {
        private ConfigurationDbContext DbContext { get; }

        public ApiSecretService(ConfigurationDbContext configurationDbContext)
        {
            DbContext = configurationDbContext;
        }

        public Task<IdentityServer4.EntityFramework.Entities.ApiSecret> Single(ApiSecretRequestModel model)
        {
            var result = DbContext.ApiResources.SelectMany(apiResource => apiResource.Secrets).SingleOrDefault(p => p.Id == int.Parse(model.Criteria));
            return Task.FromResult(result);

            //通过父类加载
            /*
            var apiResource = ConfigurationDbContext.ApiResources.Single(p => p.Id == 1);
            var goodPosts = ConfigurationDbContext.Entry(apiResource)
                .Collection(p => p.Secrets)
                .Query()
                .Where(p => p.Id > model.Id.Value);
            */
        }

        public Task<IdentityServer4.EntityFramework.Entities.ApiSecret> Add(ApiSecretRequestModel model)
        {
            var apiSecret = new IdentityServer4.EntityFramework.Entities.ApiSecret()
            {
                Description = model.ApiSecret.Description,
                Value = model.ApiSecret.Value,
                Expiration = model.ApiSecret.Expiration ?? null,
                Type = model.ApiSecret.Type,
                Created = DateTime.Now,
                ApiResourceId = model.ApiSecret.ApiResourceId
            };
            var entry = DbContext.Entry(apiSecret);
            entry.State = EntityState.Added;
            DbContext.SaveChanges();
            entry.Reload();
            return Task.FromResult(entry.Entity);
        }

        public Task<IdentityServer4.EntityFramework.Entities.ApiSecret> Modify(ApiSecretRequestModel model)
        {
            var apiSecret = new IdentityServer4.EntityFramework.Entities.ApiSecret()
            {
                Id = model.ApiSecret.Id,
                ApiResourceId = model.ApiSecret.ApiResourceId,
                Description = model.ApiSecret.Description,
                Value = model.ApiSecret.Value,
                Expiration = model.ApiSecret.Expiration ?? null,
                Type = model.ApiSecret.Type
            };
            var entry = DbContext.Entry(apiSecret);
            entry.State = EntityState.Modified;
            DbContext.SaveChanges();
            entry.Reload();
            return Task.FromResult(entry.Entity);
        }

        public Task<IdentityServer4.EntityFramework.Entities.ApiSecret> Delete(ApiSecretRequestModel model)
        {
            var apiSecret = new IdentityServer4.EntityFramework.Entities.ApiSecret() { Id = model.ApiSecret.Id };
            var entry = DbContext.Entry(apiSecret);
            entry.State = EntityState.Deleted;
            DbContext.SaveChanges();
            return Task.FromResult(apiSecret);
        }
    }
}
