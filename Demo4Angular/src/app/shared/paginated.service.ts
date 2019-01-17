import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { PaginatedRequestModel } from 'src/app/shared/request.model';
import { OperationResult, PaginatedResult } from 'src/app/shared/result';
import { Injectable } from '@angular/core';
import { BaseModel } from './base.model';
import { BaseService } from './base.service';

@Injectable()
export class PaginatedService<M extends PaginatedRequestModel, T> extends BaseService<M, T> {
    constructor(protected http: HttpClient) {
        super(http);
    }

    retrieve(requestModel: M): Observable<OperationResult<PaginatedResult<T>>> {
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
            .get<OperationResult<PaginatedResult<T>>>(requestModel.url, options)
            .pipe(catchError(this.handleError(requestModel.url)));
    }
}
