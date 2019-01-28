using Demo4DotNetCore.ResourceServer.Model;
using System;
using System.Collections.Generic;

namespace Demo4DotNetCore.ResourceServer.Identity.Model
{
    public class ApiResource : BaseModel<int>
    {
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public List<ApiSecret> Secrets { get; set; }
        public List<ApiScope> Scopes { get; set; }
        public List<ApiResourceClaim> UserClaims { get; set; }
        public List<ApiResourceProperty> Properties { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? LastAccessed { get; set; }
        public bool NonEditable { get; set; }
    }

    public class Secret : BaseModel<int>
    {
        public string Description { get; set; }
        public string Value { get; set; }
        public DateTime? Expiration { get; set; }
        public string Type { get; set; }
        public DateTime Created { get; set; }
    }

    public class ApiSecret : Secret
    {
        public int ApiResourceId { get; set; }
        public ApiResource ApiResource { get; set; }
    }

    public class UserClaim : BaseModel<int>
    {
        public string Type { get; set; }
    }

    public class ApiScopeClaim : UserClaim
    {
        public int ApiScopeId { get; set; }
        public ApiScope ApiScope { get; set; }
    }

    public class ApiScope : BaseModel<int>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public List<ApiScopeClaim> UserClaims { get; set; }
        public int ApiResourceId { get; set; }
        public ApiResource ApiResource { get; set; }
    }

    public class ApiResourceClaim : UserClaim
    {
        public int ApiResourceId { get; set; }
        public ApiResource ApiResource { get; set; }
    }

    public class Property : BaseModel<int>
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class ApiResourceProperty : Property
    {
        public int ApiResourceId { get; set; }
        public ApiResource ApiResource { get; set; }
    }
}
