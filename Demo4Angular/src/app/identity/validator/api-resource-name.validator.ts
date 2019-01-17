import { AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { ApiResourceService } from '../service/api-resource.service';

export function uniqueApiResourceNameValidatorFn(apiResourceService: ApiResourceService, id: number): AsyncValidatorFn {
    return (control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> => {
        return apiResourceService.uniqueApiResourceName(id, control.value)
            .pipe(
                map(result => (result ? { uniqueApiResourceName: control.value } : null)),
                catchError(() => null)
            );
    };
}
