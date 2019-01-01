import { DefaultConfig } from './const';

export class RequestModel {
    criteria?: string;
}

export class PagenatRequestModel extends RequestModel {
    pageIndex: number;
    pageSize: number;
    orderBy: string;
    direction: string;

    constructor() {
        super();
        this.pageIndex = DefaultConfig.PageIndex;
        this.pageSize = DefaultConfig.PageSize;
        this.orderBy = '';
        this.direction = '';
    }
}
