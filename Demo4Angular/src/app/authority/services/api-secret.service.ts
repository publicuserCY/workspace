import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ApiSecretRequestModel } from '../models/api-resource-request.model';
import { ApiSecret } from '../models/api-resource.model';
import { Uris } from 'src/app/shared/const';
import { OperationResult } from 'src/app/shared/result';
import { BaseService } from 'src/app/shared/base.service';

@Injectable()
export class ApiSecretService extends BaseService<ApiSecret> {
    constructor(protected http: HttpClient) { super(http); }

    add(requestModel: ApiSecretRequestModel): Observable<OperationResult<ApiSecret>> {
        return super.add(requestModel, Uris.AddApiSecret);
    }

    modify(requestModel: ApiSecretRequestModel): Observable<OperationResult<ApiSecret>> {
        return super.modify(requestModel, Uris.ModifyApiSecret);
    }

    delete(requestModel: ApiSecretRequestModel): Observable<OperationResult<ApiSecret>> {
        return super.delete(requestModel, Uris.DeleteApiSecret);
    }
}
