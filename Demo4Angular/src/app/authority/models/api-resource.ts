export class Property {
    id: number;
    key: string;
    value: string;
}

export class Secret {
    id: number;
    description: string;
    value: string;
    expiration?: Date;
    type: string;
    created: Date;
}

export class UserClaim {
    id: number;
    type: string;
}

export class ApiSecret extends Secret {
    apiResourceId: number;
    apiResource: ApiResource;
}

export class ApiScope {
    id: number;
    name: string;
    displayName: string;
    description: string;
    required: boolean;
    emphasize: boolean;
    showInDiscoveryDocument: boolean;
    userClaims: Array<ApiScopeClaim>;
    apiResourceId: number;
    apiResource: ApiResource;
}

export class ApiScopeClaim extends UserClaim {
    apiScopeId: number;
    apiScope: ApiScope;
}

export class ApiResourceClaim extends UserClaim {
    apiResourceId: number;
    apiResource: ApiResource;
}

export class ApiResourceProperty extends Property {
    apiResourceId: number;
    apiResource: ApiResource;
}

export class ApiResource {
    id: string;
    enabled: boolean;
    name: string;
    displayName?: string;
    description?: string;
    secrets?: Array<ApiSecret>;
    scopes?: Array<ApiScope>;
    userClaims?: Array<ApiResourceClaim>;
    properties?: Array<ApiResourceProperty>;
    created?: Date;
    updated?: Date;
    lastAccessed?: Date;
    nonEditable?: boolean;
}
