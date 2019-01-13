import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiResourceRequestModel } from '../models/api-resource-request.model';
import { ApiResource } from '../models/api-resource.model';
import { Uris } from 'src/app/shared/const';
import { OperationResult, PaginatedResult } from 'src/app/shared/result';
import { PaginatedService } from 'src/app/shared/paginated.service';

@Injectable()
export class ApiResourceService extends PaginatedService<ApiResourceRequestModel, ApiResource> {
    constructor(protected http: HttpClient) { super(http); }

    retrieve(requestModel: ApiResourceRequestModel): Observable<OperationResult<PaginatedResult<ApiResource>>> {
        this.params = new HttpParams();
        if (requestModel.enabled !== null && requestModel.enabled !== undefined) {
            this.params = this.params.set('enabled', `${requestModel.enabled}`);
        }
        if (requestModel.name && requestModel.name.trim().length > 0) {
            this.params = this.params.set('name', requestModel.name.trim());
        }
        if (requestModel.description && requestModel.description.trim().length > 0) {
            this.params = this.params.set('description', requestModel.description.trim());
        }
        if (requestModel.displayName && requestModel.displayName.trim().length > 0) {
            this.params = this.params.set('displayName', requestModel.displayName.trim());
        }
        return super.retrieve(requestModel);
    }

    uniqueApiResourceName(id: number, name: string): Observable<boolean> {
        const options = {
            params: new HttpParams()
                .set('id', id.toString())
                .set('name', name)
        };
        return this.http.get<boolean>(Uris.UniqueApiResourceName, options);
    }
}
