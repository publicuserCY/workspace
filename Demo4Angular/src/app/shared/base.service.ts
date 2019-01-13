import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { RequestModel } from 'src/app/shared/request.model';
import { OperationResult } from 'src/app/shared/result';
import { BaseModel } from './base.model';
import { ApiResourceRequestModel, ApiSecretRequestModel, ApiScopeRequestModel } from '../authority/models/api-resource-request.model';
import { Uris, EntityState } from './const';

@Injectable()
export class BaseService<M extends RequestModel, T extends BaseModel> {
    protected url: string;
    protected params: HttpParams;
    constructor(protected http: HttpClient) {
        this.params = new HttpParams();
    }

    single(requestModel: M): Observable<OperationResult<T>> {
        if (requestModel instanceof ApiResourceRequestModel) {
            this.url = Uris.SingleApiResource;
        }
        this.params = this.params.set('criteria', requestModel.criteria.trim());
        const options = { params: this.params };
        return this.http
            .get<OperationResult<T>>(this.url, options)
            .pipe(catchError(this.handleError(this.url)));
    }

    submit(requestModel: M): Observable<OperationResult<T>> {
        let state: EntityState;
        if (requestModel instanceof ApiResourceRequestModel) {
            state = requestModel.apiResource.state;
            switch (state) {
                case EntityState.Added:
                    this.url = Uris.AddApiResource;
                    break;
                case EntityState.Modified:
                    this.url = Uris.ModifyApiResource;
                    break;
                case EntityState.Deleted:
                    this.url = Uris.DeleteApiResource;
                    break;
            }
        } else if (requestModel instanceof ApiSecretRequestModel) {
            state = requestModel.apiSecret.state;
            switch (state) {
                case EntityState.Added:
                    this.url = Uris.AddApiSecret;
                    break;
                case EntityState.Modified:
                    this.url = Uris.ModifyApiSecret;
                    break;
                case EntityState.Deleted:
                    this.url = Uris.DeleteApiSecret;
                    break;
            }
        } else if (requestModel instanceof ApiScopeRequestModel) {
            state = requestModel.apiScope.state;
            switch (state) {
                case EntityState.Added:
                    this.url = Uris.AddApiScope;
                    break;
                case EntityState.Modified:
                    this.url = Uris.ModifyApiScope;
                    break;
                case EntityState.Deleted:
                    this.url = Uris.DeleteApiScope;
                    break;
            }
        }
        return this.http
            .post<OperationResult<T>>(this.url, requestModel)
            .pipe(catchError(this.handleError(this.url)));
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
