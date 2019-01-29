import { EntityState } from 'src/app/shared/const';
import { BaseModel } from 'src/app/shared/base.model';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { Property, Secret } from './api-resource.model';

export class Client extends BaseModel<number> {
    authorizationCodeLifetime: number;
    consentLifetime?: number;
    absoluteRefreshTokenLifetime: number;
    slidingRefreshTokenLifetime: number;
    refreshTokenUsage: number;
    updateAccessTokenClaimsOnRefresh: boolean;
    refreshTokenExpiration: number;
    accessTokenType: number;
    enableLocalLogin: boolean;
    identityProviderRestrictions: ClientIdPRestriction[] = [];
    accessTokenLifetime: number;
    includeJwtId: boolean;
    alwaysSendClientClaims: boolean;
    clientClaimsPrefix: string;
    pairWiseSubjectSalt: string;
    allowedCorsOrigins: ClientCorsOrigin[] = [];
    properties: ClientProperty[] = [];
    created: Date;
    updated?: Date;
    lastAccessed?: Date;
    userSsoLifetime?: number;
    userCodeType: string;
    claims: ClientClaim[] = [];
    identityTokenLifetime: number;
    allowedScopes: ClientScope[] = [];
    allowOfflineAccess: boolean;
    enabled: boolean;
    clientId: string;
    protocolType: string;
    clientSecrets: ClientSecret[] = [];
    requireClientSecret: boolean;
    clientName: string;
    description: string;
    clientUri: string;
    logoUri: string;
    requireConsent: boolean;
    allowRememberConsent: boolean;
    alwaysIncludeUserClaimsInIdToken: boolean;
    allowedGrantTypes: ClientGrantType[] = [];
    requirePkce: boolean;
    allowPlainTextPkce: boolean;
    allowAccessTokensViaBrowser: boolean;
    redirectUris: ClientRedirectUri[] = [];
    postLogoutRedirectUris: ClientPostLogoutRedirectUri[] = [];
    frontChannelLogoutUri: string;
    frontChannelLogoutSessionRequired: boolean;
    backChannelLogoutUri: string;
    backChannelLogoutSessionRequired: boolean;
    deviceCodeLifetime: number;
    nonEditable: boolean;

    constructor() {
        super();
    }
}
export class ClientIdPRestriction extends BaseModel<number> {
    provider: string;
    clientId: number;
}
export class ClientCorsOrigin extends BaseModel<number> {
    origin: string;
    clientId: number;
}
export class ClientProperty extends Property {
    clientId: number;
}
export class ClientClaim extends BaseModel<number> {
    type: string;
    value: string;
    clientId: number;
}
export class ClientScope extends BaseModel<number> {
    scope: string;
    clientId: number;
}
export class ClientSecret extends Secret {
    clientId: number;
}

export class ClientGrantType extends BaseModel<number> {
    grantType: string;
    clientId: number;
}
export class ClientRedirectUri extends BaseModel<number> {
    redirectUri: string;
    clientId: number;
}
export class ClientPostLogoutRedirectUri extends BaseModel<number> {
    postLogoutRedirectUri: string;
    clientId: number;
}
