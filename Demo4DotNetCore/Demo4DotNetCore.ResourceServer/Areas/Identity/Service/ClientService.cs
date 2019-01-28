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
    public class ClientService : IClientService
    {
        private ConfigurationDbContext DbContext { get; }

        public ClientService(ConfigurationDbContext configurationDbContext)
        {
            DbContext = configurationDbContext;
        }

        public Task<PaginatedResult<IdentityServer4.EntityFramework.Entities.Client>> Retrieve(ClientRequestModel model)
        {
            var query = DbContext.Clients.AsQueryable();
            var predicate = PredicateBuilder.New<IdentityServer4.EntityFramework.Entities.Client>();
            if (model.Enabled.HasValue)
            {
                predicate = predicate.And(p => p.Enabled == model.Enabled.Value);
                query = query.AsExpandable().Where(predicate);
            }
            if (!string.IsNullOrEmpty(model.ClientId))
            {
                predicate = predicate.And(p => !string.IsNullOrEmpty(p.ClientId) && p.Description.Contains(model.ClientId, StringComparison.OrdinalIgnoreCase));
                query = query.AsExpandable().Where(predicate);
            }
            if (!string.IsNullOrEmpty(model.ClientName))
            {
                predicate = predicate.And(p => !string.IsNullOrEmpty(p.ClientName) && p.ClientName.Contains(model.ClientName, StringComparison.OrdinalIgnoreCase));
                query = query.AsExpandable().Where(predicate);
            }
            var result = query.SortBy(model.SortExpression).ToPaginatedList(model.PageIndex, model.PageSize);
            return Task.FromResult(result);
        }

        public Task<IdentityServer4.EntityFramework.Entities.Client> Single(ClientRequestModel model)
        {
            var result = DbContext.Clients
                    .Include(p => p.IdentityProviderRestrictions)
                    .Include(p => p.AllowedCorsOrigins)
                    .Include(p => p.Properties)
                    .Include(p => p.Claims)
                    .Include(p => p.AllowedScopes)
                    .Include(p => p.ClientSecrets)
                    .Include(p => p.AllowedGrantTypes)
                    .Include(p => p.RedirectUris)
                    .Include(p => p.PostLogoutRedirectUris)
                    .SingleOrDefault(p => p.Id == int.Parse(model.Criteria));
            return Task.FromResult(result);
        }

        public Task<IdentityServer4.EntityFramework.Entities.Client> Add(ClientRequestModel model)
        {
            var client = new IdentityServer4.EntityFramework.Entities.Client()
            {
                Enabled = model.Client.Enabled,
                ClientId = model.Client.ClientId,
                ProtocolType = model.Client.ProtocolType,
                RequireClientSecret = model.Client.RequireClientSecret,
                ClientName = model.Client.ClientName,
                Description = model.Client.Description,
                ClientUri = model.Client.ClientUri,
                LogoUri = model.Client.LogoUri,
                RequireConsent = model.Client.RequireConsent,
                AllowRememberConsent = model.Client.AllowRememberConsent,
                AlwaysIncludeUserClaimsInIdToken = model.Client.AlwaysIncludeUserClaimsInIdToken,
                RequirePkce = model.Client.RequirePkce,
                AllowPlainTextPkce = model.Client.AllowPlainTextPkce,
                AllowAccessTokensViaBrowser = model.Client.AllowAccessTokensViaBrowser,
                FrontChannelLogoutUri = model.Client.FrontChannelLogoutUri,
                FrontChannelLogoutSessionRequired = model.Client.FrontChannelLogoutSessionRequired,
                BackChannelLogoutUri = model.Client.BackChannelLogoutUri,
                BackChannelLogoutSessionRequired = model.Client.BackChannelLogoutSessionRequired,
                AllowOfflineAccess = model.Client.AllowOfflineAccess,
                IdentityTokenLifetime = model.Client.IdentityTokenLifetime,
                AccessTokenLifetime = model.Client.AccessTokenLifetime,
                AuthorizationCodeLifetime = model.Client.AuthorizationCodeLifetime,
                ConsentLifetime = model.Client.ConsentLifetime,
                AbsoluteRefreshTokenLifetime = model.Client.AbsoluteRefreshTokenLifetime,
                SlidingRefreshTokenLifetime = model.Client.SlidingRefreshTokenLifetime,
                RefreshTokenUsage = model.Client.RefreshTokenUsage,
                UpdateAccessTokenClaimsOnRefresh = model.Client.UpdateAccessTokenClaimsOnRefresh,
                RefreshTokenExpiration = model.Client.RefreshTokenExpiration,
                AccessTokenType = model.Client.AccessTokenType,
                EnableLocalLogin = model.Client.EnableLocalLogin,
                IncludeJwtId = model.Client.IncludeJwtId,
                AlwaysSendClientClaims = model.Client.AlwaysSendClientClaims,
                ClientClaimsPrefix = model.Client.ClientClaimsPrefix,
                PairWiseSubjectSalt = model.Client.PairWiseSubjectSalt,
                Created = DateTime.Now,
                Updated = null,
                LastAccessed = null,
                UserSsoLifetime = model.Client.UserSsoLifetime,
                UserCodeType = model.Client.UserCodeType,
                DeviceCodeLifetime = model.Client.DeviceCodeLifetime,
                NonEditable = model.Client.NonEditable
            };
            var entry = DbContext.Entry(client);
            entry.State = EntityState.Added;
            DbContext.SaveChanges();
            entry.Reload();
            return Task.FromResult(entry.Entity);
        }

        public Task<IdentityServer4.EntityFramework.Entities.Client> Modify(ClientRequestModel model)
        {
            var client = new IdentityServer4.EntityFramework.Entities.Client()
            {
                Id = model.Client.Id,
                Enabled = model.Client.Enabled,
                ClientId = model.Client.ClientId,
                ProtocolType = model.Client.ProtocolType,
                RequireClientSecret = model.Client.RequireClientSecret,
                ClientName = model.Client.ClientName,
                Description = model.Client.Description,
                ClientUri = model.Client.ClientUri,
                LogoUri = model.Client.LogoUri,
                RequireConsent = model.Client.RequireConsent,
                AllowRememberConsent = model.Client.AllowRememberConsent,
                AlwaysIncludeUserClaimsInIdToken = model.Client.AlwaysIncludeUserClaimsInIdToken,
                RequirePkce = model.Client.RequirePkce,
                AllowPlainTextPkce = model.Client.AllowPlainTextPkce,
                AllowAccessTokensViaBrowser = model.Client.AllowAccessTokensViaBrowser,
                FrontChannelLogoutUri = model.Client.FrontChannelLogoutUri,
                FrontChannelLogoutSessionRequired = model.Client.FrontChannelLogoutSessionRequired,
                BackChannelLogoutUri = model.Client.BackChannelLogoutUri,
                BackChannelLogoutSessionRequired = model.Client.BackChannelLogoutSessionRequired,
                AllowOfflineAccess = model.Client.AllowOfflineAccess,
                IdentityTokenLifetime = model.Client.IdentityTokenLifetime,
                AccessTokenLifetime = model.Client.AccessTokenLifetime,
                AuthorizationCodeLifetime = model.Client.AuthorizationCodeLifetime,
                ConsentLifetime = model.Client.ConsentLifetime,
                AbsoluteRefreshTokenLifetime = model.Client.AbsoluteRefreshTokenLifetime,
                SlidingRefreshTokenLifetime = model.Client.SlidingRefreshTokenLifetime,
                RefreshTokenUsage = model.Client.RefreshTokenUsage,
                UpdateAccessTokenClaimsOnRefresh = model.Client.UpdateAccessTokenClaimsOnRefresh,
                RefreshTokenExpiration = model.Client.RefreshTokenExpiration,
                AccessTokenType = model.Client.AccessTokenType,
                EnableLocalLogin = model.Client.EnableLocalLogin,
                IncludeJwtId = model.Client.IncludeJwtId,
                AlwaysSendClientClaims = model.Client.AlwaysSendClientClaims,
                ClientClaimsPrefix = model.Client.ClientClaimsPrefix,
                PairWiseSubjectSalt = model.Client.PairWiseSubjectSalt,
                Created = model.Client.Created,
                Updated = DateTime.Now,
                LastAccessed = model.Client.LastAccessed,
                UserSsoLifetime = model.Client.UserSsoLifetime,
                UserCodeType = model.Client.UserCodeType,
                DeviceCodeLifetime = model.Client.DeviceCodeLifetime,
                NonEditable = model.Client.NonEditable
            };
            var entry = DbContext.Entry(client);
            entry.State = EntityState.Modified;
            DbContext.SaveChanges();
            entry.Reload();
            return Task.FromResult(entry.Entity);
        }

        public Task<IdentityServer4.EntityFramework.Entities.Client> Delete(ClientRequestModel model)
        {
            var client = new IdentityServer4.EntityFramework.Entities.Client() { Id = model.Client.Id };
            var entry = DbContext.Entry(client);
            entry.State = EntityState.Deleted;
            DbContext.SaveChanges();
            return Task.FromResult(entry.Entity);
        }

        public Task<bool> UniqueClientId(int id, string clientId)
        {
            var result = DbContext.Clients.Any(p => !p.Id.Equals(id) && p.ClientId.Equals(clientId, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(result);
        }
    }
}
