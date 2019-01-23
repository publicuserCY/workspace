import { EntityState } from './const';
import * as uuidV4 from 'uuid/v4';
import { FormBuilder, AbstractControl, Validators } from '@angular/forms';

export abstract class BaseModel<T> {
    sid = uuidV4();
    id: T;
    state = EntityState.Unchanged;
    constructor() { }

    toControl(fb: FormBuilder): AbstractControl {
        const control = fb.group({ id: [null, Validators.required] });
        return control;
    }

    /* UUID(len: number, radix: number) {
        const chars = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz'.split('');
        // tslint:disable-next-line:prefer-const
        let uuid = [], i: number;
        radix = radix || chars.length;

        if (len) {
            // Compact form
            // tslint:disable-next-line:no-bitwise
            for (i = 0; i < len; i++) { uuid[i] = chars[0 | Math.random() * radix]; }
        } else {
            // rfc4122, version 4 form
            let r;

            // rfc4122 requires these characters
            uuid[8] = uuid[13] = uuid[18] = uuid[23] = '-';
            uuid[14] = '4';

            // Fill in random data. At i==19 set the high bits of clock sequence as
            // per rfc4122, sec. 4.1.5
            for (i = 0; i < 36; i++) {
                if (!uuid[i]) {
                    // tslint:disable-next-line:no-bitwise
                    r = 0 | Math.random() * 16;
                    // tslint:disable-next-line:no-bitwise
                    uuid[i] = chars[(i === 19) ? (r & 0x3) | 0x8 : r];
                }
            }
        }

        return uuid.join('');
    } */
}
