import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { RequestModel, PaginatedRequestModel } from 'src/app/shared/request.model';
import { OperationResult, PaginatedResult } from 'src/app/shared/result';
import { Injectable } from '@angular/core';
import { BaseModel } from './base.model';
import { ApiResourceRequestModel } from '../authority/models/api-resource-request.model';
import { Uris } from './const';
import { BaseService } from './base.service';

@Injectable()
export class PaginatedService<M extends PaginatedRequestModel, T extends BaseModel> extends BaseService<M, T> {
    constructor(protected http: HttpClient) {
        super(http);
    }

    retrieve(requestModel: M): Observable<OperationResult<PaginatedResult<T>>> {
        if (requestModel instanceof ApiResourceRequestModel) {
            this.url = Uris.RetrieveApiResource;
        }
        this.params = this.params.set('pageIndex', `${requestModel.pageIndex}`);
        this.params = this.params.set('pageSize', `${requestModel.pageSize}`);
        if (requestModel.orderBy && requestModel.orderBy.trim().length > 0) {
            this.params = this.params.set('orderBy', requestModel.orderBy.trim());
        }
        if (requestModel.direction && requestModel.direction.trim().length > 0) {
            this.params = this.params.set('direction', requestModel.direction.trim());
        }
        if (requestModel.criteria && requestModel.criteria.trim().length > 0) {
            this.params = this.params.set('criteria', requestModel.criteria.trim());
        }
        const options = { params: this.params };
        return this.http
            .get<OperationResult<PaginatedResult<T>>>(this.url, options)
            .pipe(catchError(this.handleError(this.url)));
    }
}
