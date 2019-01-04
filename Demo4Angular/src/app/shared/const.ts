const AuthServer = 'http://localhost:5000';
const ResourceServer = 'http://localhost:5001';
export const Uris = {
    DiscoveryEndpoint: AuthServer + '/.well-known/openid-configuratio',
    AuthorizeEndpoint: AuthServer + '/connect/authorize',
    TokenEndpoint: AuthServer + '/connect/token',
    SelectApiResource: AuthServer + '/api/Identity/SelectApiResource',
    InsertApiResource: AuthServer + '/api/Identity/InsertApiResource',
    UpdateApiResource: AuthServer + '/api/Identity/UpdateApiResource',
    DeleteApiResource: AuthServer + '/api/Identity/DeleteApiResource',
    UniqueApiResourceName: AuthServer + '/api/Identity/UniqueApiResourceName',
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
