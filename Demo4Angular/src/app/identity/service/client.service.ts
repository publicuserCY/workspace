import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Client } from '../model/client.model';
import { ClientRequestModel } from '../model/client-request.model';
import { Uris } from 'src/app/shared/const';
import { OperationResult, PaginatedResult } from 'src/app/shared/result';
import { PaginatedService } from 'src/app/shared/paginated.service';

@Injectable()
export class ClientService extends PaginatedService<ClientRequestModel, Client> {
    constructor(protected http: HttpClient) { super(http); }

    retrieve(requestModel: ClientRequestModel): Observable<OperationResult<PaginatedResult<Client>>> {
        this.params = new HttpParams();
        if (requestModel.enabled !== null && requestModel.enabled !== undefined) {
            this.params = this.params.set('enabled', `${requestModel.enabled}`);
        }
        if (requestModel.clientId !== null && requestModel.clientId !== undefined) {
            this.params = this.params.set('clientId', requestModel.clientId);
        }
        if (requestModel.clientName !== null && requestModel.clientName !== undefined) {
            this.params = this.params.set('clientName', requestModel.clientName);
        }
        return super.retrieve(requestModel);
    }

    uniqueClientId(id: number, clientId: string): Observable<boolean> {
        const options = {
            params: new HttpParams()
                .set('id', id.toString())
                .set('clientId', clientId)
        };
        return this.http.get<boolean>(Uris.UniqueClientId, options);
    }
}
