import { PagenatRequestModel } from 'src/app/common/request';

export class ApiResourceRequestModel extends PagenatRequestModel {
    id: number;
    enabled: boolean;
    name: string;
    displayName: string;
    description: string;
    nonEditable: boolean;
    constructor() {
        super();
        this.id = 0;
        this.enabled = true;
        this.name = '';
        this.displayName = '';
        this.description = '';
        this.nonEditable = false;
    }
}
