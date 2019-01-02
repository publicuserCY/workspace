import { AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { AuthorityService } from '../services/authority.service';

export function uniqueApiResourceNameValidatorFn(authorityService: AuthorityService, id: number): AsyncValidatorFn {
    return (control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> => {
        return authorityService.uniqueApiResourceName(id, control.value)
            .pipe(
                map(result => (result ? { uniqueApiResourceName: control.value } : null)),
                catchError(() => null)
            );
    };
}
