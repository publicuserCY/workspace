import { EntityState } from 'src/app/shared/const';
import { BaseModel } from 'src/app/shared/base.model';

export class ApiResource extends BaseModel {
    id: number;
    enabled: boolean;
    name: string;
    displayName: string;
    description: string;
    secrets: ApiSecret[] = [];
    scopes: ApiScope[] = [];
    userClaims: ApiResourceClaim[] = [];
    properties: ApiResourceProperty[] = [];
    created?: Date;
    updated?: Date;
    lastAccessed?: Date;
    nonEditable: boolean;

    constructor() {
        super();
        this.id = 0;
        this.enabled = true;
        this.name = '';
        this.created = new Date();
        this.nonEditable = false;
    }

    private getNewApiSecretId(): number {
        const sorted = this.secrets.sort((a: { id: number; }, b: { id: number; }) => b.id - a.id);
        if (sorted.length > 0) {
            return sorted[0].id + 1;
        }
        return 1;
    }

    addApiSecret(item: ApiSecret) {
        item.id = this.getNewApiSecretId();
        item.apiResourceId = this.id;
        item.state = EntityState.Added;
        this.secrets = [...this.secrets, item];
    }
    removeApiSecret(item: ApiSecret) {
        const index = this.secrets.findIndex(p => p.id === item.id);
        this.secrets[index].state = EntityState.Deleted;
    }
    updateApiSecret(item: ApiSecret) {
        const index = this.secrets.findIndex(p => p.id === item.id);
        item.state = EntityState.Modified;
        Object.assign(this.secrets[index], item);
    }
}

abstract class Secret extends BaseModel {
    id: number;
    description?: string;
    value: string;
    expiration?: Date;
    type: string;
    created: Date;

    constructor() {
        super();
        this.id = 0;
        this.value = '';
        this.type = '';
        this.created = new Date();
    }
}

export class ApiSecret extends Secret {
    apiResourceId: number;
    constructor() {
        super();
        this.apiResourceId = 0;
    }
}

abstract class UserClaim extends BaseModel {
    id: number;
    type: string;
    constructor() {
        super();
        this.id = 0;
        this.type = '';
    }
}

export class ApiScopeClaim extends UserClaim {
    apiScopeId: number;
    constructor() {
        super();
    }
}

export class ApiScope extends BaseModel {
    id: number;
    name: string;
    displayName: string;
    description: string;
    required: boolean;
    emphasize: boolean;
    showInDiscoveryDocument: boolean;
    userClaims: Array<ApiScopeClaim>;
    apiResourceId: number;
    constructor() {
        super();
        this.id = 0;
        this.name = '';
        this.required = false;
        this.emphasize = false;
        this.showInDiscoveryDocument = false;
    }
}

export class ApiResourceClaim extends UserClaim {
    apiResourceId: number;
    constructor() {
        super();
    }
}

abstract class Property extends BaseModel {
    id: number;
    key: string;
    value: string;
    constructor() {
        super();
        this.id = 0;
        this.key = '';
        this.value = '';
    }
}

export class ApiResourceProperty extends Property {
    apiResourceId: number;
    constructor() {
        super();
    }
}
