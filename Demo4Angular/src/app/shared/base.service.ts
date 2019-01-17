import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { RequestModel } from 'src/app/shared/request.model';
import { OperationResult } from 'src/app/shared/result';
import { BaseModel } from './base.model';

@Injectable()
export class BaseService<M extends RequestModel, T> {
    protected params: HttpParams;
    constructor(protected http: HttpClient) {
        this.params = new HttpParams();
    }

    single(requestModel: M): Observable<OperationResult<T>> {
        this.params = this.params.set('criteria', requestModel.criteria.trim());
        const options = { params: this.params };
        return this.http
            .get<OperationResult<T>>(requestModel.url, options)
            .pipe(catchError(this.handleError(requestModel.url)));
    }

    submit(requestModel: M): Observable<OperationResult<T>> {
        return this.http
            .post<OperationResult<T>>(requestModel.url, requestModel)
            .pipe(catchError(this.handleError(requestModel.url)));
    }

    /**
  * Handle Http operation that failed.
  * Let the app continue.
  * @param operation - name of the operation that failed
  * @param result - optional value to return as the observable result
  */
    protected handleError(operation = 'operation') {
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
