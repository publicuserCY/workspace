using Demo4DotNetCore.AuthorizationServer.Model;

namespace Demo4DotNetCore.AuthorizationServer.RequestModel
{
    public class ApiResourceRequestModel : PaginatedRequestModel
    {
        public int? Id { get; set; }
        public bool? Enabled { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public ApiResource ApiResource { get; set; }
    }

    //public abstract class SecretRequestModel : BaseRequestModel
    //{
    //    public Secret Secret { get; set; }
    //}

    public class ApiSecretRequestModel : BaseRequestModel
    {
        public ApiSecret ApiSecret { get; set; }
    }

    public abstract class UserClaimRequestModel : BaseRequestModel
    {
        public UserClaim UserClaim { get; set; }
    }

    public class ApiScopeClaimRequestModel : UserClaimRequestModel
    {
        public ApiScopeClaim ApiScopeClaim { get; set; }
    }

    public class ApiScopeRequestModel : BaseRequestModel
    {
        public int? Id { get; set; }
        public ApiScope ApiScope { get; set; }
    }

    //public class ApiResourceClaimRequestModel : UserClaimRequestModel
    //{
    //    public ApiResourceClaim ApiResourceClaim { get; set; }
    //}

    //public abstract class PropertyRequestModel
    //{
    //    public Property Property { get; set; }
    //}

    //public class ApiResourcePropertyRequestModel : PropertyRequestModel
    //{
    //    public ApiResourceProperty ApiResourceProperty { get; set; }
    //}
}
