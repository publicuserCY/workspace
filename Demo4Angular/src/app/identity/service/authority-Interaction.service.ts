import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

import { ApiSecret, ApiScope } from '../model/api-resource.model';

@Injectable()
export class AuthorityInteractionService {
    private apiSecretSource = new Subject<ApiSecret>();
    private apiScopeSource = new Subject<ApiScope>();
    apiSecret$ = this.apiSecretSource.asObservable();
    apiScope$ = this.apiScopeSource.asObservable();

    apiSecretChanged(item: ApiSecret) {
        this.apiSecretSource.next(item);
    }
    apiScopeChanged(item: ApiScope) {
        this.apiScopeSource.next(item);
    }
}
