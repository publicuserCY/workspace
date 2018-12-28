import { AuthConfig } from 'angular-oauth2-oidc';

export const roAuthConfig: AuthConfig = {

    // Url of the Identity Provider
    issuer: 'http://localhost:5000',

    // URL of the SPA to redirect the user to after login
    redirectUri: window.location.origin + '/home',
    // The SPA's id. The SPA is registerd with this id at the auth-server
    clientId: 'resource-owner-client',

    // set the scope for the permissions the client should request
    // The first three are defined by OIDC. The 4th is a usecase-specific one
    scope: 'openid profile resapi offline_access',
    oidc: false
};

export const implicitAuthConfig: AuthConfig = {

    // Url of the Identity Provider
    issuer: 'http://localhost:5000',

    // URL of the SPA to redirect the user to after login
    redirectUri: window.location.origin + '/home',
    // dummyClientSecret: 'secret',
    // The SPA's id. The SPA is registerd with this id at the auth-server
    clientId: 'implicit-client',

    // set the scope for the permissions the client should request
    // The first three are defined by OIDC. The 4th is a usecase-specific one
    scope: 'openid profile resapi',
};
