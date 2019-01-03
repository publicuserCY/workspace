import { RequestModel, PagenatRequestModel, Operational } from 'src/app/common/request.model';

export class ApiResourceRequestModel extends PagenatRequestModel {
    id: number;
    enabled: boolean;
    name: string;
    displayName: string;
    description: string;
    secrets: ApiSecretRequestModel[] = [];
    scopes: ApiScopeRequestModel[] = [];
    userClaims: ApiResourceClaimRequestModel[] = [];
    properties: ApiResourcePropertyRequestModel[] = [];
    created?: Date;
    updated?: Date;
    lastAccessed?: Date;
    nonEditable: boolean;
    constructor(pageIndex?: number, pageSize?: number, orderBy?: string, direction?: string) {
        super(pageIndex, pageSize, orderBy, direction);
        this.flag = Operational.Insert;
        this.id = 0;
        this.enabled = true;
        this.name = '';
        this.created = new Date();
        this.nonEditable = false;
    }

    getNewApiSecretId(): number {
        const sorted = this.secrets.sort((a: { id: number; }, b: { id: number; }) => b.id - a.id);
        if (sorted.length > 0) {
            return sorted[0].id + 1;
        }
        return 1;
    }

    addApiSecret(item: ApiSecretRequestModel) {
        item.id = this.getNewApiSecretId();
        item.apiResourceId = this.id;
        item.flag = Operational.Insert;
        this.secrets = [...this.secrets, item];
    }
    removeApiSecret(item: ApiSecretRequestModel) {
        const index = this.secrets.findIndex(p => p.id === item.id);
        this.secrets[index].flag = Operational.Delete;
        // this.secrets = this.secrets.filter(p => p.flag !== Operational.Delete);
    }
    updateApiSecret(item: ApiSecretRequestModel) {
        item.flag = Operational.Update;
        const index = this.secrets.findIndex(p => p.id === item.id);
        Object.assign(this.secrets[index], item);
    }
}

abstract class SecretRequestModel extends RequestModel {
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

export class ApiSecretRequestModel extends SecretRequestModel {
    apiResourceId: number;
    constructor() {
        super();
        this.apiResourceId = 0;
    }
}

abstract class UserClaimRequestModel extends RequestModel {
    id: number;
    type: string;
    constructor() {
        super();
        this.id = 0;
        this.type = '';
    }
}

export class ApiScopeClaimRequestModel extends UserClaimRequestModel {
    apiScopeId: number;
    constructor() {
        super();
    }
}

export class ApiScopeRequestModel extends RequestModel {
    id: number;
    name: string;
    displayName: string;
    description: string;
    required: boolean;
    emphasize: boolean;
    showInDiscoveryDocument: boolean;
    userClaims: Array<ApiScopeClaimRequestModel>;
    apiResourceId: number;
    constructor() {
        super();
    }
}

export class ApiResourceClaimRequestModel extends UserClaimRequestModel {
    apiResourceId: number;
    constructor() {
        super();
    }
}

abstract class PropertyRequestModel extends RequestModel {
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

export class ApiResourcePropertyRequestModel extends PropertyRequestModel {
    apiResourceId: number;
    constructor() {
        super();
    }
}
