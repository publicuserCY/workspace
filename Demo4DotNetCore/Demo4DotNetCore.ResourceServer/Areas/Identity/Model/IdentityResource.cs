using Demo4DotNetCore.ResourceServer.Model;
using System;
using System.Collections.Generic;

namespace Demo4DotNetCore.ResourceServer.Identity.Model
{
    public class IdentityResource : BaseModel<int>
    {
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public List<IdentityClaim> UserClaims { get; set; }
        public List<IdentityResourceProperty> Properties { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool NonEditable { get; set; }
    }

    public class IdentityClaim : UserClaim
    {
        public int IdentityResourceId { get; set; }
    }

    public class IdentityResourceProperty : Property
    {
        public int IdentityResourceId { get; set; }
    }
}
