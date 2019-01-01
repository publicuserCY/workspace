const AuthServer = 'http://localhost:5000';
const ResourceServer = 'http://localhost:5001';
export const Uris = {
    DiscoveryEndpoint: AuthServer + '/.well-known/openid-configuratio',
    AuthorizeEndpoint: AuthServer + '/connect/authorize',
    TokenEndpoint: AuthServer + '/connect/token',
    SelectApiResource: AuthServer + '/api/Identity/SelectApiResource',
    InsertApiResource: AuthServer + '/api/Identity/InsertApiResource',
    UpdateApiResource: AuthServer + '/api/Identity/UpdateApiResource',
    DeleteApiResource: AuthServer + '/api/Identity/DeletetApiResource',
    UniqueApiResourceName: AuthServer + '/api/Identity/UniqueApiResourceName',
};
export const DefaultConfig = {
    PageIndex: 1,
    PageSize: 10,
};
