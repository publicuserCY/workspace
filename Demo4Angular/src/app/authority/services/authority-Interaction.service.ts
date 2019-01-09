import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

import { ApiSecret, ApiScope } from '../models/api-resource.model';

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

    private apiScopeAddedSource = new Subject<ApiScope>();
    private apiScopeModifiedSource = new Subject<ApiScope>();
    private apiScopeDeletedSource = new Subject<ApiScope>();
    apiScopeAdded$ = this.apiScopeAddedSource.asObservable();
    apiScopeModified$ = this.apiScopeModifiedSource.asObservable();
    apiScopeDeleted$ = this.apiScopeDeletedSource.asObservable();


    /* apiSecretShifted(items: Array<ApiSecret>) {
        this.apiSecretsSource.next(items);
    } */

    apiSecretAdded(item: ApiSecret) {
        this.apiSecretAddedSource.next(item);
    }

    apiSecretModified(item: ApiSecret) {
        this.apiSecretModifiedSource.next(item);
    }

    apiSecretDeleted(item: ApiSecret) {
        this.apiSecretDeletedSource.next(item);
    }

    apiScopeAdded(item: ApiScope) {
        this.apiScopeAddedSource.next(item);
    }

    apiScopeModified(item: ApiScope) {
        this.apiScopeModifiedSource.next(item);
    }

    apiScopeDeleted(item: ApiScope) {
        this.apiScopeDeletedSource.next(item);
    }
}
