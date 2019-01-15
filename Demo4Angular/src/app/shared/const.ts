// export const AuthServer = 'http://192.168.11.62:5000';
// export const AuthServer = 'http://192.168.11.12:5000';
export const AuthServer = 'http://localhost:5000';
// export const AuthServer = 'http://120.79.33.159:5000';
// export const ResourceServer = 'https://192.168.11.12:5001';
export const Uris = {
    DiscoveryEndpoint: AuthServer + '/.well-known/openid-configuration',
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
    UniqueApiScopeName: AuthServer + '/api/ApiScope/UniqueApiScopeName',

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
