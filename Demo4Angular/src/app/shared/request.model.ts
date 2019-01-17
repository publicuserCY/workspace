import { DefaultConfig } from './const';

export abstract class RequestModel {
    url: string;
    criteria?: string;
    constructor(url: string) {
        this.url = url;
        this.criteria = '';
    }
}

export class PaginatedRequestModel extends RequestModel {
    pageIndex = 1;
    pageSize = DefaultConfig.PageSize;
    orderBy = '';
    direction = 'asc';

    constructor(url: string, pageIndex?: number, pageSize?: number, orderBy?: string, direction?: string) {
        super(url);
        if (pageIndex) { this.pageIndex = pageIndex; }
        if (pageSize) { this.pageSize = pageSize; }
        if (orderBy) { this.orderBy = orderBy; }
        if (direction) {
            this.direction = direction.indexOf('asc') >= 0 ? 'asc' : 'desc';
        }
    }
}
