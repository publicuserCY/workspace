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

    RetrieveClient: ResourceServer + '/api/Identity/Client/Retrieve',
    SingleClient: ResourceServer + '/api/Identity/Client/Single',
    AddClient: ResourceServer + '/api/Identity/Client/Add',
    ModifyClient: ResourceServer + '/api/Identity/Client/Modify',
    DeleteClient: ResourceServer + '/api/Identity/Client/Delete',
    UniqueClientId: ResourceServer + '/api/Identity/Client/UniqueClientId',
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
