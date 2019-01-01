import { Injectable } from '@angular/core';
import { AsyncValidator, AbstractControl, ValidationErrors } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { AuthorityService } from '../services/authority.service';

@Injectable()
export class UniqueApiResourceNameValidator implements AsyncValidator {
    constructor(private authorityService: AuthorityService) { }

    validate(ctrl: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> {
        console.log(this);
        return this.authorityService.uniqueApiResourceName(ctrl.value)
            .pipe(
                map(result => (result ? { uniqueApiResourceName: ctrl.value } : null)),
                catchError(() => null)
            );
    }
}
