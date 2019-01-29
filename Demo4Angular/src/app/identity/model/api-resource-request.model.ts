import { RequestModel, PaginatedRequestModel, } from 'src/app/shared/request.model';
import { ApiResource, ApiScope, ApiSecret } from './api-resource.model';

export class ApiResourceRequestModel extends PaginatedRequestModel {
    apiResource: ApiResource;
    enabled?: boolean;
    name?: string;
    displayName?: string;
    description?: string;

    constructor(url: string, pageIndex?: number, pageSize?: number, orderBy?: string, direction?: string) {
        super(url, pageIndex, pageSize, orderBy, direction);
    }
}

export class ApiScopeRequestModel extends RequestModel {
    apiScope: ApiScope;

    constructor(url: string) {
        super(url);
    }
}

export class ApiSecretRequestModel extends PaginatedRequestModel {
    apiSecret: ApiSecret;

    constructor(url: string) {
        super(url);
    }
}

