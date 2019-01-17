import { EntityState } from 'src/app/shared/const';
import { BaseModel } from 'src/app/shared/base.model';

export class ApiResource extends BaseModel<number> {
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
        this.nonEditable = false;
    }

    /* private getNewApiSecretId(): number {
        const sorted = this.secrets.sort((a: { id: number; }, b: { id: number; }) => b.id - a.id);
        if (sorted.length > 0) {
            return sorted[0].id + 1;
        }
        return 1;
    }

    private getNewApiScopeId(): number {
        const sorted = this.scopes.sort((a: { id: number; }, b: { id: number; }) => b.id - a.id);
        if (sorted.length > 0) {
            return sorted[0].id + 1;
        }
        return 1;
    } */

    /*** ApiSecret ***/
    addApiSecret(item: ApiSecret) {
        item.id = 0;
        item.apiResourceId = this.id;
        item.state = EntityState.Added;
        this.secrets = [...this.secrets, item];
    }
    modifyApiSecret(item: ApiSecret) {
        const index = this.secrets.findIndex(p => p.id === item.id);
        item.state = EntityState.Modified;
        Object.assign(this.secrets[index], item);
    }
    deleteApiSecret(item: ApiSecret) {
        this.secrets = this.secrets.filter(p => p.id !== item.id);
    }

    /*** ApiScope ***/
    addApiScope(item: ApiScope) {
        item.id = 0;
        item.apiResourceId = this.id;
        item.state = EntityState.Added;
        this.scopes = [...this.scopes, item];
    }
    modifyApiScope(item: ApiScope) {
        const index = this.scopes.findIndex(p => p.id === item.id);
        item.state = EntityState.Modified;
        Object.assign(this.scopes[index], item);
    }
    deleteApiScope(item: ApiScope) {
        this.scopes = this.scopes.filter(p => p.id !== item.id);
    }

}

abstract class Secret extends BaseModel<number> {
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
    }
}

export class ApiSecret extends Secret {
    apiResourceId: number;
    constructor() {
        super();
        this.apiResourceId = 0;
    }
}

abstract class UserClaim extends BaseModel<number> {
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

export class ApiScope extends BaseModel<number> {
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

abstract class Property extends BaseModel<number> {
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
