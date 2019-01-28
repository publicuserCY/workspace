using Demo4DotNetCore.ResourceServer.Model;
using System;
using System.Collections.Generic;

namespace Demo4DotNetCore.ResourceServer.Identity.Model
{
    public class Client : BaseModel<int>
    {
        public int AuthorizationCodeLifetime { get; set; }
        public int? ConsentLifetime { get; set; }
        public int AbsoluteRefreshTokenLifetime { get; set; }
        public int SlidingRefreshTokenLifetime { get; set; }
        public int RefreshTokenUsage { get; set; }
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public int AccessTokenType { get; set; }
        public bool EnableLocalLogin { get; set; }
        public List<ClientIdPRestriction> IdentityProviderRestrictions { get; set; }
        public int AccessTokenLifetime { get; set; }
        public bool IncludeJwtId { get; set; }
        public bool AlwaysSendClientClaims { get; set; }
        public string ClientClaimsPrefix { get; set; }
        public string PairWiseSubjectSalt { get; set; }
        public List<ClientCorsOrigin> AllowedCorsOrigins { get; set; }
        public List<ClientProperty> Properties { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? LastAccessed { get; set; }
        public int? UserSsoLifetime { get; set; }
        public string UserCodeType { get; set; }
        public List<ClientClaim> Claims { get; set; }
        public int IdentityTokenLifetime { get; set; }
        public List<ClientScope> AllowedScopes { get; set; }
        public bool AllowOfflineAccess { get; set; }
        public bool Enabled { get; set; }
        public string ClientId { get; set; }
        public string ProtocolType { get; set; }
        public List<ClientSecret> ClientSecrets { get; set; }
        public bool RequireClientSecret { get; set; }
        public string ClientName { get; set; }
        public string Description { get; set; }
        public string ClientUri { get; set; }
        public string LogoUri { get; set; }
        public bool RequireConsent { get; set; }
        public bool AllowRememberConsent { get; set; }
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
        public List<ClientGrantType> AllowedGrantTypes { get; set; }
        public bool RequirePkce { get; set; }
        public bool AllowPlainTextPkce { get; set; }
        public bool AllowAccessTokensViaBrowser { get; set; }
        public List<ClientRedirectUri> RedirectUris { get; set; }
        public List<ClientPostLogoutRedirectUri> PostLogoutRedirectUris { get; set; }
        public string FrontChannelLogoutUri { get; set; }
        public bool FrontChannelLogoutSessionRequired { get; set; }
        public string BackChannelLogoutUri { get; set; }
        public bool BackChannelLogoutSessionRequired { get; set; }
        public int DeviceCodeLifetime { get; set; }
        public bool NonEditable { get; set; }
    }

    public class ClientIdPRestriction : BaseModel<int>
    {
        public string Provider { get; set; }
        public int ClientId { get; set; }
    }

    public class ClientCorsOrigin : BaseModel<int>
    {
        public string Origin { get; set; }
        public int ClientId { get; set; }
    }

    public class ClientProperty : Property
    {
        public int ClientId { get; set; }
    }

    public class ClientClaim : BaseModel<int>
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public int ClientId { get; set; }
    }

    public class ClientScope : BaseModel<int>
    {
        public string Scope { get; set; }
        public int ClientId { get; set; }
    }

    public class ClientSecret : Secret
    {
        public int ClientId { get; set; }
    }

    public class ClientGrantType : BaseModel<int>
    {
        public string GrantType { get; set; }
        public int ClientId { get; set; }
    }

    public class ClientRedirectUri : BaseModel<int>
    {
        public string RedirectUri { get; set; }
        public int ClientId { get; set; }
    }

    public class ClientPostLogoutRedirectUri : BaseModel<int>
    {
        public string PostLogoutRedirectUri { get; set; }
        public int ClientId { get; set; }
    }
}
