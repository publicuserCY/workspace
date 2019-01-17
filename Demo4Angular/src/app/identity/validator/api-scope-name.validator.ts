import { AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { ApiScopeService } from '../service/api-scope.service';

export function uniqueApiScopeNameValidatorFn(apiScopeService: ApiScopeService, id: number): AsyncValidatorFn {
    return (control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> => {
        return apiScopeService.uniqueApiScopeName(id, control.value)
            .pipe(
                map(result => (result ? { uniqueApiScopeName: control.value } : null)),
                catchError(() => null)
            );
    };
}
