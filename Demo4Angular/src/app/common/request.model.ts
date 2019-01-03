import { DefaultConfig } from './const';

export enum Operational {
    Delete = -1, Origin = 0, Update = 1, Insert = 2
}

export abstract class RequestModel {
    criteria?: string;
    flag = Operational.Origin;
    constructor() {
        this.flag = Operational.Insert;
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
