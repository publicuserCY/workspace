import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { OperationResult } from 'src/app/common/result';
import { ApiResourceRequestModel } from '../models/request';
import { ApiResource } from '../models/api-resource';
import { Uris } from 'src/app/common/const';


@Injectable()
export class AuthorityService {
    constructor(private http: HttpClient) { }

    selectApiResource(model: ApiResourceRequestModel): Observable<OperationResult<ApiResource[]>> {
        const options = {
            params: new HttpParams()
                .set('id', model.id.toString())
                .set('criteria', model.criteria ? model.criteria.trim() : '')
                .set('pageIndex', model.pageIndex.toString())
                .set('pageSize', model.pageSize.toString())
                .set('orderBy', model.orderBy)
                .set('direction', model.direction)
                .set('name', model.name ? model.name.trim() : '')
                .set('description', model.description ? model.description.trim() : '')
        };
        return this.http
            .get<OperationResult<ApiResource[]>>(Uris.SelectApiResource, options)
            .pipe(catchError(this.handleError('selectApiResource')));
    }

    insertApiResource(model: ApiResourceRequestModel): Observable<OperationResult<ApiResource>> {
        return this.http
            .post<OperationResult<ApiResource>>(Uris.InsertApiResource, model)
            .pipe(catchError(this.handleError('insertApiResource')));
    }

    uniqueApiResourceName(name: string): Observable<boolean> {
        const options = {
            params: new HttpParams().set('name', name)
        };
        return this.http.get<boolean>(Uris.UniqueApiResourceName, options);
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
