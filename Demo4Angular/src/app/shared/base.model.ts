import { EntityState } from './const';

export abstract class BaseModel {
    state = EntityState.Unchanged;
    constructor() {
        this.state = EntityState.Added;
    }
}
