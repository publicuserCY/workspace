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
    constructor() {
        this.description = '';
        this.value = '';
        this.type = 'cc';
        this.created = new Date();
    }
}

export class UserClaim {
    id: number;
    type: string;
}

export class ApiSecret extends Secret {
    apiResourceId: number;
    apiResource: ApiResource;
    constructor(parent: ApiResource) {
        super();
        this.id = 1;
        this.apiResourceId = parent.id;
        this.apiResource = parent;
        const sorted = parent.secrets.sort((a, b) => b.id - a.id);
        if (sorted && sorted.length > 0) {
            this.id = sorted[0].id + 1;
        }
    }
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
    id: number;
    enabled: boolean;
    name: string;
    displayName: string;
    description: string;
    secrets: Array<ApiSecret>;
    scopes: Array<ApiScope>;
    userClaims: Array<ApiResourceClaim>;
    properties: Array<ApiResourceProperty>;
    created: Date;
    updated: Date;
    lastAccessed: Date;
    nonEditable: boolean;

    constructor() {
        this.id = 0;
        this.enabled = true;
        this.secrets = [];
        this.scopes = [];
        this.userClaims = [];
        this.properties = [];
    }
}
