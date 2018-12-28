using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo4DotNetCore.AuthorizationServer.Model
{
    public class ApiResourceRequestModel : PaginatedRequestModel
    {
        public int Id { get; set; }
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
}
