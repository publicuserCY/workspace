import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';

import { ApiResourceRequestModel, ApiSecretRequestModel } from '../models/api-resource-request.model';
import { ApiResource, ApiSecret } from '../models/api-resource.model';
import { Uris } from 'src/app/shared/const';
import { OperationResult, PaginatedResult } from 'src/app/shared/result';


@Injectable()
export class AuthorityService {
    constructor(private http: HttpClient) { }

    retrieveApiResource(requestModel: ApiResourceRequestModel): Observable<OperationResult<PaginatedResult<ApiResource>>> {
        let params = new HttpParams();
        params = params.set('pageIndex', `${requestModel.pageIndex}`);
        params = params.set('pageSize', `${requestModel.pageSize}`);
        if (requestModel.orderBy && requestModel.orderBy.trim().length > 0) {
            params = params.set('orderBy', requestModel.orderBy.trim());
        }
        if (requestModel.direction && requestModel.direction.trim().length > 0) {
            params = params.set('direction', requestModel.direction.trim());
        }
        if (requestModel.criteria && requestModel.criteria.trim().length > 0) {
            params = params.set('criteria', requestModel.criteria.trim());
        }
        if (requestModel.id) {
            params = params.set('id', `${requestModel.id}`);
        }
        if (requestModel.enabled !== null && requestModel.enabled !== undefined) {
            params = params.set('enabled', `${requestModel.enabled}`);
        }
        if (requestModel.name && requestModel.name.trim().length > 0) {
            params = params.set('name', requestModel.name.trim());
        }
        if (requestModel.description && requestModel.description.trim().length > 0) {
            params = params.set('description', requestModel.description.trim());
        }
        if (requestModel.displayName && requestModel.displayName.trim().length > 0) {
            params = params.set('displayName', requestModel.displayName.trim());
        }
        const options = { params: params };
        return this.http
            .get<OperationResult<PaginatedResult<ApiResource>>>(Uris.RetrieveApiResource, options)
            .pipe(catchError(this.handleError('retrieveApiResource')));
    }
    /*** Api Resource ***/
    addApiResource(requestModel: ApiResourceRequestModel): Observable<OperationResult<ApiResource>> {
        return this.http
            .post<OperationResult<ApiResource>>(Uris.AddApiResource, requestModel)
            .pipe(catchError(this.handleError('addApiResource')));
    }

    modifyApiResource(requestModel: ApiResourceRequestModel): Observable<OperationResult<ApiResource>> {
        return this.http
            .post<OperationResult<ApiResource>>(Uris.ModifyApiResource, requestModel)
            .pipe(catchError(this.handleError('modifyApiResource')));
    }

    deleteApiResource(requestModel: ApiResourceRequestModel): Observable<OperationResult<ApiResource>> {
        return this.http
            .post<OperationResult<ApiResource>>(Uris.DeleteApiResource, requestModel)
            .pipe(catchError(this.handleError('deleteApiResource')));
    }

    uniqueApiResourceName(id: number, name: string): Observable<boolean> {
        const options = {
            params: new HttpParams()
                .set('id', id.toString())
                .set('name', name)
        };
        return this.http.get<boolean>(Uris.UniqueApiResourceName, options);
    }
    /*** Api Secret ***/
    addApiSecret(requestModel: ApiSecretRequestModel): Observable<OperationResult<ApiSecret>> {
        return this.http
            .post<OperationResult<ApiSecret>>(Uris.AddApiSecret, requestModel)
            .pipe(catchError(this.handleError('addApiSecret')));
    }

    modifyApiSecret(requestModel: ApiSecretRequestModel): Observable<OperationResult<ApiSecret>> {
        return this.http
            .post<OperationResult<ApiSecret>>(Uris.ModifyApiSecret, requestModel)
            .pipe(catchError(this.handleError('modifyApiSecret')));
    }

    deleteApiSecret(requestModel: ApiSecretRequestModel): Observable<OperationResult<ApiSecret>> {
        return this.http
            .post<OperationResult<ApiSecret>>(Uris.DeleteApiSecret, requestModel)
            .pipe(catchError(this.handleError('deleteApiSecret')));
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
