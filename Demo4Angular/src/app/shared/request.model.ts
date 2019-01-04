import { DefaultConfig, EntityState } from './const';

export abstract class RequestModel {
    criteria?: string;
    state = EntityState.Unchanged;
    constructor() {
        this.state = EntityState.Added;
    }
}

export class PagenatRequestModel extends RequestModel {
    pageIndex = 1;
    pageSize = DefaultConfig.PageSize;
    orderBy = '';
    direction = '';

    constructor(pageIndex?: number, pageSize?: number, orderBy?: string, direction?: string) {
        super();
        if (pageIndex) { this.pageIndex = pageIndex; }
        if (pageSize) { this.pageSize = pageSize; }
        if (orderBy) { this.orderBy = orderBy; }
        if (direction) { this.direction = direction; }
    }
}
