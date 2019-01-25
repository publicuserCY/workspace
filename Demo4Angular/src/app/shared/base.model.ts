import { EntityState } from './const';
import * as uuidV4 from 'uuid/v4';

export abstract class BaseModel<T> {
    sid = uuidV4();
    id: T;
    state = EntityState.Unchanged;
    constructor() { }
}
