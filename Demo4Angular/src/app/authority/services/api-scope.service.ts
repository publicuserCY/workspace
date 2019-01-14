import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiScopeRequestModel } from '../models/api-resource-request.model';
import { ApiScope } from '../models/api-resource.model';
import { Uris } from 'src/app/shared/const';
import { BaseService } from 'src/app/shared/base.service';

@Injectable()
export class ApiScopeService extends BaseService<ApiScopeRequestModel, ApiScope> {
    constructor(protected http: HttpClient) { super(http); }

    uniqueApiScopeName(id: number, name: string): Observable<boolean> {
        const options = {
            params: new HttpParams()
                .set('id', id.toString())
                .set('name', name)
        };
        return this.http.get<boolean>(Uris.UniqueApiScopeName, options);
    }
}
