using System;
using System.Collections.Generic;

namespace Demo4DotNetCore.AuthorizationServer.Model
{
    public class ApiResourceRequestModel : PaginatedRequestModel
    {
        public int Id { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public List<ApiSecretRequestModel> Secrets { get; set; }
        public List<ApiScopeRequestModel> Scopes { get; set; }
        public List<ApiResourceClaimRequestModel> UserClaims { get; set; }
        public List<ApiResourcePropertyRequestModel> Properties { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? LastAccessed { get; set; }
        public bool NonEditable { get; set; }
    }

    public abstract class SecretRequestModel : BaseRequestModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public DateTime? Expiration { get; set; }
        public string Type { get; set; }
        public DateTime Created { get; set; }
    }

    public class ApiSecretRequestModel : SecretRequestModel
    {
        public int ApiResourceId { get; set; }
    }

    public abstract class UserClaimRequestModel : BaseRequestModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }

    public class ApiScopeClaimRequestModel : UserClaimRequestModel
    {
        public int ApiScopeId { get; set; }
    }

    public class ApiScopeRequestModel : BaseRequestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public List<ApiScopeClaimRequestModel> UserClaims { get; set; }
        public int ApiResourceId { get; set; }
    }

    public class ApiResourceClaimRequestModel : UserClaimRequestModel
    {
        public int ApiResourceId { get; set; }
    }

    public abstract class PropertyRequestModel
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class ApiResourcePropertyRequestModel : PropertyRequestModel
    {
        public int ApiResourceId { get; set; }
    }
}
