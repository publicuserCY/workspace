import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { ApiResourceRequestModel, ApiSecretRequestModel } from '../models/api-resource-request.model';

@Injectable()
export class AuthorityInteractionService {
    private apiSecretSource = new Subject<Array<ApiSecretRequestModel>>();
    private apiSecretAddSource = new Subject<ApiSecretRequestModel>();
    private apiSecretUpdateSource = new Subject<ApiSecretRequestModel>();
    private apiSecretDelelteSource = new Subject<ApiSecretRequestModel>();
    apiSecrets$ = this.apiSecretSource.asObservable();
    apiSecretAdd$ = this.apiSecretAddSource.asObservable();
    apiSecretUpdate$ = this.apiSecretUpdateSource.asObservable();
    apiSecretDelete$ = this.apiSecretDelelteSource.asObservable();

    touchapiSecrets(items: Array<ApiSecretRequestModel>) {
        this.apiSecretSource.next(items);
    }

    apiSecretAdded(item: ApiSecretRequestModel) {
        this.apiSecretAddSource.next(item);
    }

    apiSecretUpdated(item: ApiSecretRequestModel) {
        this.apiSecretUpdateSource.next(item);
    }

    apiSecretDelete(item: ApiSecretRequestModel) {
        this.apiSecretDelelteSource.next(item);
    }
}
