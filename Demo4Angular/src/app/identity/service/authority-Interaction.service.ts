import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

import { ApiSecret, ApiScope, ApiScopeClaim } from '../model/api-resource.model';

@Injectable()
export class AuthorityInteractionService {
    private apiSecretSource = new Subject<ApiSecret>();
    private apiScopeSource = new Subject<ApiScope>();
    private apiScopeClaimSource = new Subject<any>();
    apiSecret$ = this.apiSecretSource.asObservable();
    apiScope$ = this.apiScopeSource.asObservable();
    apiScopeClaim$ = this.apiScopeClaimSource.asObservable();

    apiSecretChanged(item: ApiSecret) {
        this.apiSecretSource.next(item);
    }
    apiScopeChanged(item: ApiScope) {
        this.apiScopeSource.next(item);
    }
    apiScopeClaimChanged(item: any) {
        this.apiScopeClaimSource.next(item);
    }

}
