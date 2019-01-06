import { DefaultConfig } from './const';

export abstract class RequestModel {
    criteria?: string;
    constructor() {
        this.criteria = '';
    }
}

export class PaginatedRequestModel extends RequestModel {
    pageIndex = 1;
    pageSize = DefaultConfig.PageSize;
    orderBy = '';
    direction = '';

    constructor(pageIndex?: number, pageSize?: number, orderBy?: string, direction?: string) {
        super();
        if (pageIndex) { this.pageIndex = pageIndex; }
        if (pageSize) { this.pageSize = pageSize; }
        if (orderBy) { this.orderBy = orderBy; }
        if (direction) {
            this.direction = direction.indexOf('asc') >= 0 ? 'asc' : 'desc';
        }
    }
}
