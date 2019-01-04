import { PagenatRequestModel } from 'src/app/shared/request.model';
import { ApiResource } from './api-resource.model';

export class ApiResourceRequestModel extends PagenatRequestModel {
    id: number;
    name: string;
    description: string;
    apiResource: ApiResource;

    constructor(pageIndex?: number, pageSize?: number, orderBy?: string, direction?: string) {
        super(pageIndex, pageSize, orderBy, direction);
        this.id = 0;
        this.name = '';
    }
}
