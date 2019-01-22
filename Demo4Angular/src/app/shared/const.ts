/* export const AuthServer = 'https://192.168.11.12:44300';
export const ResourceServer = 'https://192.168.11.12:44301'; */
export const AuthServer = 'http://localhost:5000';
export const ResourceServer = 'http://localhost:5001';
/* export const AuthServer = 'https://120.79.33.159:44300';
export const ResourceServer = 'https://120.79.33.159:44301'; */
export const Uris = {
    DiscoveryEndpoint: AuthServer + '/.well-known/openid-configuration',
    AuthorizeEndpoint: AuthServer + '/connect/authorize',
    TokenEndpoint: AuthServer + '/connect/token',

    RetrieveApiResource: ResourceServer + '/api/Identity/ApiResource/Retrieve',
    SingleApiResource: ResourceServer + '/api/Identity/ApiResource/Single',
    AddApiResource: ResourceServer + '/api/Identity/ApiResource/Add',
    ModifyApiResource: ResourceServer + '/api/Identity/ApiResource/Modify',
    DeleteApiResource: ResourceServer + '/api/Identity/ApiResource/Delete',
    UniqueApiResourceName: ResourceServer + '/api/Identity/ApiResource/UniqueApiResourceName',
    UniqueApiScopeName: ResourceServer + '/api/Identity/ApiResource/UniqueApiScopeName',

    /* AddApiScope: ResourceServer + '/api/Identity/ApiScope/Add',
    ModifyApiScope: ResourceServer + '/api/Identity/ApiScope/Modify',
    DeleteApiScope: ResourceServer + '/api/Identity/ApiScope/Delete',

    AddApiScopeClaim: ResourceServer + '/api/Identity/ApiScopeClaim/Add',
    ModifyApiScopeClaim: ResourceServer + '/api/Identity/ApiScopeClaim/Modify',
    DeleteApiScopeClaim: ResourceServer + '/api/Identity/ApiScopeClaim/Delete',
    UniqueApiScopeName: ResourceServer + '/api/Identity/ApiScope/UniqueApiScopeName',

    AddApiSecret: ResourceServer + '/api/Identity/ApiSecret/Add',
    ModifyApiSecret: ResourceServer + '/api/Identity/ApiSecret/Modify',
    DeleteApiSecret: ResourceServer + '/api/Identity/ApiSecret/Delete', */
};
export const DefaultConfig = {
    PageSize: 10,
};

export enum EntityState {
    Unchanged = 1,
    Deleted = 2,
    Modified = 3,
    Added = 4
}
