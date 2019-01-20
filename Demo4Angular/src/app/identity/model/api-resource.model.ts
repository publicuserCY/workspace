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
    }

    static assign(des: ApiResource, src: ApiResource): ApiResource {
        des.state = src.state;
        des.id = src.id;
        des.enabled = src.enabled;
        des.name = src.name;
        des.displayName = src.displayName;
        des.description = src.description;
        des.created = src.created;
        des.updated = src.updated;
        des.lastAccessed = src.lastAccessed;
        des.nonEditable = src.nonEditable;
        src.secrets.forEach(item => {
            const target = des.secrets.find(p => p.id === item.id);
            if (target) {
                ApiSecret.assign(target, item);
            } else {
                des.secrets.push(ApiSecret.assign(new ApiSecret(), item));
            }
        });
        src.scopes.forEach(item => {
            const target = des.scopes.find(p => p.id === item.id);
            if (target) {
                ApiScope.assign(target, item);
            } else {
                des.scopes.push(ApiScope.assign(new ApiScope(), item));
            }
        });
        src.userClaims.forEach(item => {
            const target = des.userClaims.find(p => p.id === item.id);
            if (target) {
                ApiResourceClaim.assign(target, item);
            } else {
                des.userClaims.push(ApiResourceClaim.assign(new ApiResourceClaim(), item));
            }
        });
        src.properties.forEach(item => {
            const target = des.properties.find(p => p.id === item.id);
            if (target) {
                ApiResourceProperty.assign(target, item);
            } else {
                des.properties.push(ApiResourceProperty.assign(new ApiResourceProperty(), item));
            }
        });
        return des;
    }

    /* private getNewApiSecretId(): number {
        const sorted = this.secrets.sort((a: { id: number; }, b: { id: number; }) => b.id - a.id);
        if (sorted.length > 0) {
            return sorted[0].id + 1;
        }
        return 1;
    } */

    /*** secrets ***/
    addSecret(item: ApiSecret) {
        item.apiResourceId = this.id;
        item.state = EntityState.Added;
        this.secrets = [...this.secrets, item];
    }
    modifySecret(item: ApiSecret) {
        const target = this.secrets.find(p => p.sid === item.sid);
        target.state = EntityState.Modified;
        ApiSecret.assign(target, item);
    }
    deleteSecret(item: ApiSecret) {
        this.secrets = this.secrets.filter(p => p.sid !== item.sid);
    }

    /*** scopes ***/
    addScope(item: ApiScope) {
        item.apiResourceId = this.id;
        item.state = EntityState.Added;
        this.scopes = [...this.scopes, item];
    }
    modifyScope(item: ApiScope) {
        const target = this.scopes.find(p => p.sid === item.sid);
        target.state = EntityState.Modified;
        ApiScope.assign(target, item);
    }
    deleteScope(item: ApiScope) {
        this.scopes = this.scopes.filter(p => p.sid !== item.sid);
    }

    /*** userClaims ***/
    addUserClaim(item: ApiResourceClaim) {
        item.apiResourceId = this.id;
        item.state = EntityState.Added;
        this.userClaims = [...this.userClaims, item];
    }
    modifyUserClaim(item: ApiResourceClaim) {
        const target = this.userClaims.find(p => p.sid === item.sid);
        target.state = EntityState.Modified;
        ApiResourceClaim.assign(target, item);
    }
    deleteUserClaim(item: ApiResourceClaim) {
        this.userClaims = this.userClaims.filter(p => p.sid !== item.sid);
    }

    /*** properties ***/
    addProperty(item: ApiResourceProperty) {
        item.apiResourceId = this.id;
        item.state = EntityState.Added;
        this.properties = [...this.properties, item];
    }
    modifyProperty(item: ApiResourceProperty) {
        const target = this.properties.find(p => p.sid === item.sid);
        target.state = EntityState.Modified;
        ApiResourceProperty.assign(target, item);
    }
    deleteProperty(item: ApiResourceProperty) {
        this.properties = this.properties.filter(p => p.sid !== item.sid);
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
    }
}

export class ApiSecret extends Secret {
    apiResourceId: number;
    constructor() {
        super();
    }
    static assign(des: ApiSecret, src: ApiSecret): ApiSecret {
        des.id = src.id;
        des.apiResourceId = src.apiResourceId;
        des.description = src.description;
        des.value = src.value;
        des.expiration = src.expiration;
        des.type = src.type;
        des.created = src.created;
        return des;
    }
}

abstract class UserClaim extends BaseModel<number> {
    type: string;
    constructor() {
        super();
    }
}

export class ApiScopeClaim extends UserClaim {
    apiScopeId: number;
    constructor() {
        super();
    }
    static assign(des: ApiScopeClaim, src: ApiScopeClaim): ApiScopeClaim {
        des.id = src.id;
        des.apiScopeId = src.apiScopeId;
        des.type = src.type;
        return des;
    }
}

export class ApiScope extends BaseModel<number> {
    name: string;
    displayName: string;
    description: string;
    required: boolean;
    emphasize: boolean;
    showInDiscoveryDocument: boolean;
    userClaims: ApiScopeClaim[] = [];
    apiResourceId: number;
    constructor() {
        super();
        this.id = 0;
        this.required = false;
        this.emphasize = false;
        this.showInDiscoveryDocument = false;
    }
    static assign(des: ApiScope, src: ApiScope): ApiScope {
        des.id = src.id;
        des.apiResourceId = src.apiResourceId;
        des.name = src.name;
        des.displayName = src.displayName;
        des.description = src.description;
        des.required = src.required;
        des.emphasize = src.emphasize;
        des.showInDiscoveryDocument = src.showInDiscoveryDocument;
        src.userClaims.forEach(item => {
            const target = des.userClaims.find(p => p.id === item.id);
            if (target) {
                ApiScopeClaim.assign(target, item);
            } else {
                des.userClaims.push(ApiScopeClaim.assign(new ApiScopeClaim(), item));
            }
        });
        return des;
    }
    addScopeClaim(item: ApiScopeClaim) {
        item.apiScopeId = this.id;
        item.state = EntityState.Added;
        this.userClaims = [...this.userClaims, item];
    }
    modifyScopeClaim(item: ApiScopeClaim) {
        const target = this.userClaims.find(p => p.sid === item.sid);
        target.state = EntityState.Modified;
        ApiScopeClaim.assign(target, item);
    }
    deleteScopeClaim(item: ApiScopeClaim) {
        item.state = EntityState.Deleted;
    }
}

export class ApiResourceClaim extends UserClaim {
    apiResourceId: number;
    constructor() {
        super();
    }
    static assign(des: ApiResourceClaim, src: ApiResourceClaim): ApiResourceClaim {
        des.id = src.id;
        des.apiResourceId = src.apiResourceId;
        des.type = src.type;
        return des;
    }
}

abstract class Property extends BaseModel<number> {
    key: string;
    value: string;
    constructor() {
        super();
    }
}

export class ApiResourceProperty extends Property {
    apiResourceId: number;
    constructor() {
        super();
    }
    static assign(des: ApiResourceProperty, src: ApiResourceProperty): ApiResourceProperty {
        des.id = src.id;
        des.apiResourceId = src.apiResourceId;
        des.key = src.key;
        des.value = src.value;
        return des;
    }
}
