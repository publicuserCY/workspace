import { EntityState } from './const';

export abstract class BaseModel<T> {
    id: T;
    state = EntityState.Unchanged;
    constructor() {
        this.state = EntityState.Added;
    }
}
