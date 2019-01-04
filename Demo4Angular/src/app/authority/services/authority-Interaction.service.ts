import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

import { ApiSecret } from '../models/api-resource.model';

@Injectable()
export class AuthorityInteractionService {
    private apiSecretsSource = new Subject<Array<ApiSecret>>();
    private apiSecretAddedSource = new Subject<ApiSecret>();
    private apiSecretModifiedSource = new Subject<ApiSecret>();
    private apiSecretDeletedSource = new Subject<ApiSecret>();
    apiSecrets$ = this.apiSecretsSource.asObservable();
    apiSecretAdded$ = this.apiSecretAddedSource.asObservable();
    apiSecretModified$ = this.apiSecretModifiedSource.asObservable();
    apiSecretDeleted$ = this.apiSecretDeletedSource.asObservable();

    apiSecretShifted(items: Array<ApiSecret>) {
        this.apiSecretsSource.next(items);
    }

    apiSecretAdded(item: ApiSecret) {
        this.apiSecretAddedSource.next(item);
    }

    apiSecretUpdated(item: ApiSecret) {
        this.apiSecretModifiedSource.next(item);
    }

    apiSecretDeleted(item: ApiSecret) {
        this.apiSecretDeletedSource.next(item);
    }
}
