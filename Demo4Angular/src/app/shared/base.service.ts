import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { RequestModel, PaginatedRequestModel } from 'src/app/shared/request.model';
import { OperationResult, PaginatedResult } from 'src/app/shared/result';

export abstract class BaseService<T> {
    protected params: HttpParams;
    protected http: HttpClient;
    constructor(http: HttpClient) {
        this.params = new HttpParams();
        this.http = http;
    }

    protected retrieve(requestModel: PaginatedRequestModel, url?: string): Observable<OperationResult<PaginatedResult<T>>> {
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
            .get<OperationResult<PaginatedResult<T>>>(url, options)
            .pipe(catchError(this.handleError('retrieve')));
    }

    protected add(requestModel: RequestModel, url?: string): Observable<OperationResult<T>> {
        return this.http
            .post<OperationResult<T>>(url, requestModel)
            .pipe(catchError(this.handleError('add')));
    }
    protected modify(requestModel: RequestModel, url?: string): Observable<OperationResult<T>> {
        return this.http
            .post<OperationResult<T>>(url, requestModel)
            .pipe(catchError(this.handleError('modify')));
    }

    protected delete(requestModel: RequestModel, url?: string): Observable<OperationResult<T>> {
        return this.http
            .post<OperationResult<T>>(url, requestModel)
            .pipe(catchError(this.handleError('delete')));
    }
    /**
  * Handle Http operation that failed.
  * Let the app continue.
  * @param operation - name of the operation that failed
  * @param result - optional value to return as the observable result
  */
    private handleError(operation = 'operation') {
        return (error: HttpErrorResponse): Observable<OperationResult<any>> => {
            const result: OperationResult<any> = {
                isSuccess: false,
                message: error.message
            };
            // Let the app keep running by returning an empty result.
            return of(result);
        };
    }
}
