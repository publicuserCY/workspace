import { AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { ApiResourceService } from '../service/api-resource.service';

export function uniqueApiScopeNameValidatorFn(apiResourceService: ApiResourceService, id: number): AsyncValidatorFn {
    return (control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> => {
        return apiResourceService.uniqueApiScopeName(id, control.value)
            .pipe(
                map(result => (result ? { uniqueApiScopeName: control.value } : null)),
                catchError(() => null)
            );
    };
}
