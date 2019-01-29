import { RequestModel, PaginatedRequestModel, } from 'src/app/shared/request.model';
import { ApiResource, ApiScope, ApiSecret } from './api-resource.model';
import { Client } from './client.model';

export class ClientRequestModel extends PaginatedRequestModel {
    client: Client;
    enabled?: boolean;
    clientId?: string;
    clientName?: string;

    constructor(url: string, pageIndex?: number, pageSize?: number, orderBy?: string, direction?: string) {
        super(url, pageIndex, pageSize, orderBy, direction);
    }
}

