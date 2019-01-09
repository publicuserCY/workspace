import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { RequestModel, PaginatedRequestModel } from 'src/app/shared/request.model';
import { OperationResult, PaginatedResult } from 'src/app/shared/result';
import { BaseModel } from './base.model';
import { ApiResourceRequestModel, ApiSecretRequestModel, ApiScopeRequestModel } from '../authority/models/api-resource-request.model';
import { Uris } from './const';

@Injectable()
export class BaseService<M extends RequestModel, T extends BaseModel> {
    protected url: string;
    protected params: HttpParams;
    constructor(protected http: HttpClient) {
        this.params = new HttpParams();
    }

    single(requestModel: M): Observable<OperationResult<T>> {
        if (requestModel.criteria && requestModel.criteria.trim().length > 0) {
            this.params = this.params.set('criteria', requestModel.criteria.trim());
        }
        const options = { params: this.params };
        return this.http
            .get<OperationResult<T>>(this.url, options)
            .pipe(catchError(this.handleError('single')));
    }

    add(requestModel: M): Observable<OperationResult<T>> {
        if (requestModel instanceof ApiResourceRequestModel) {
            this.url = Uris.AddApiResource;
        } else if (requestModel instanceof ApiSecretRequestModel) {
            this.url = Uris.AddApiSecret;
        } else if (requestModel instanceof ApiScopeRequestModel) {
            this.url = Uris.AddApiScope;
        }
        return this.http
            .post<OperationResult<T>>(this.url, requestModel)
            .pipe(catchError(this.handleError('add')));
    }
    modify(requestModel: M): Observable<OperationResult<T>> {
        if (requestModel instanceof ApiResourceRequestModel) {
            this.url = Uris.ModifyApiResource;
        } else if (requestModel instanceof ApiSecretRequestModel) {
            this.url = Uris.ModifyApiSecret;
        } else if (requestModel instanceof ApiScopeRequestModel) {
            this.url = Uris.ModifyApiScope;
        }
        return this.http
            .post<OperationResult<T>>(this.url, requestModel)
            .pipe(catchError(this.handleError('modify')));
    }

    delete(requestModel: M): Observable<OperationResult<T>> {
        if (requestModel instanceof ApiResourceRequestModel) {
            this.url = Uris.DeleteApiResource;
        } else if (requestModel instanceof ApiSecretRequestModel) {
            this.url = Uris.DeleteApiSecret;
        } else if (requestModel instanceof ApiScopeRequestModel) {
            this.url = Uris.DeleteApiScope;
        }
        return this.http
            .post<OperationResult<T>>(this.url, requestModel)
            .pipe(catchError(this.handleError('delete')));
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
