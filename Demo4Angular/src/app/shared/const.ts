const AuthServer = 'http://localhost:5000';
const ResourceServer = 'http://localhost:5001';
export const Uris = {
    DiscoveryEndpoint: AuthServer + '/.well-known/openid-configuratio',
    AuthorizeEndpoint: AuthServer + '/connect/authorize',
    TokenEndpoint: AuthServer + '/connect/token',

    RetrieveApiResource: AuthServer + '/api/ApiResource/Retrieve',
    SingleApiResource: AuthServer + '/api/ApiResource/Single',
    AddApiResource: AuthServer + '/api/ApiResource/Add',
    ModifyApiResource: AuthServer + '/api/ApiResource/Modify',
    DeleteApiResource: AuthServer + '/api/ApiResource/Delete',
    UniqueApiResourceName: AuthServer + '/api/ApiResource/UniqueApiResourceName',

    AddApiScope: AuthServer + '/api/ApiScope/Add',
    ModifyApiScope: AuthServer + '/api/ApiScope/Modify',
    DeleteApiScope: AuthServer + '/api/ApiScope/Delete',

    AddApiScopeClaim: AuthServer + '/api/ApiScopeClaim/Add',
    ModifyApiScopeClaim: AuthServer + '/api/ApiScopeClaim/Modify',
    DeleteApiScopeClaim: AuthServer + '/api/ApiScopeClaim/Delete',

    AddApiSecret: AuthServer + '/api/ApiSecret/Add',
    ModifyApiSecret: AuthServer + '/api/ApiSecret/Modify',
    DeleteApiSecret: AuthServer + '/api/ApiSecret/Delete',
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
