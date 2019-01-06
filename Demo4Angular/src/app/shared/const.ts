const AuthServer = 'http://localhost:5000';
const ResourceServer = 'http://localhost:5001';
export const Uris = {
    DiscoveryEndpoint: AuthServer + '/.well-known/openid-configuratio',
    AuthorizeEndpoint: AuthServer + '/connect/authorize',
    TokenEndpoint: AuthServer + '/connect/token',
    RetrieveApiResource: AuthServer + '/api/Identity/RetrieveApiResource',
    AddApiResource: AuthServer + '/api/Identity/AddApiResource',
    ModifyApiResource: AuthServer + '/api/Identity/ModifyApiResource',
    DeleteApiResource: AuthServer + '/api/Identity/DeleteApiResource',
    UniqueApiResourceName: AuthServer + '/api/Identity/UniqueApiResourceName',
    AddApiSecret: AuthServer + '/api/Identity/AddApiSecret',
    ModifyApiSecret: AuthServer + '/api/Identity/ModifyApiSecret',
    DeleteApiSecret: AuthServer + '/api/Identity/DeleteApiSecret',
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
