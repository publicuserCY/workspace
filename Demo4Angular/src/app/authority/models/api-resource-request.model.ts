import { RequestModel, PaginatedRequestModel, } from 'src/app/shared/request.model';
import { ApiResource, ApiScope, ApiSecret } from './api-resource.model';

export class ApiResourceRequestModel extends PaginatedRequestModel {
    enabled?: boolean;
    name?: string;
    displayName?: string;
    description?: string;
    apiResource: ApiResource;

    constructor(pageIndex?: number, pageSize?: number, orderBy?: string, direction?: string) {
        super(pageIndex, pageSize, orderBy, direction);
    }
}

export class ApiScopeRequestModel extends RequestModel {
    apiScope: ApiScope;

    constructor() {
        super();
    }
}

export class ApiSecretRequestModel extends PaginatedRequestModel {
    apiSecret: ApiSecret;

    constructor() {
        super();
    }
}

