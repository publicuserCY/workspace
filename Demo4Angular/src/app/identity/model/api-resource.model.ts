import { EntityState } from 'src/app/shared/const';
import { BaseModel } from 'src/app/shared/base.model';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';

export class ApiResource extends BaseModel<number> {
    enabled: boolean;
    name: string;
    displayName: string;
    description: string;
    secrets: ApiSecret[] = [];
    scopes: ApiScope[] = [];
    userClaims: ApiResourceClaim[] = [];
    properties: ApiResourceProperty[] = [];
    created?: Date;
    updated?: Date;
    lastAccessed?: Date;
    nonEditable: boolean;

    constructor() {
        super();
    }

    static toControl(fb: FormBuilder, src: ApiResource): FormGroup {
        src.state = EntityState.Unchanged;
        const control = fb.group({
            state: [src.state],
            id: [src.id, Validators.required],
            enabled: [src.enabled, Validators.required],
            name: [src.name, { validators: [Validators.required], updateOn: 'blur' }],
            displayName: [src.displayName],
            description: [src.description],
            secrets: fb.array([]),
            scopes: fb.array([]),
            userClaims: fb.array([]),
            properties: fb.array([]),
            created: [src.created, Validators.required],
            updated: [src.updated],
            lastAccessed: [src.lastAccessed],
            nonEditable: [src.nonEditable, Validators.required]
        });
        src.secrets.forEach(item => {
            (control.get('secrets') as FormArray).push(ApiSecret.toControl(fb, item));
        });
        src.scopes.forEach(item => {
            (control.get('scopes') as FormArray).push(ApiScope.toControl(fb, item));
        });
        src.userClaims.forEach(item => {
            (control.get('userClaims') as FormArray).push(ApiResourceClaim.toControl(fb, item));
        });
        src.properties.forEach(item => {
            (control.get('properties') as FormArray).push(ApiResourceProperty.toControl(fb, item));
        });
        return control;
    }

    static fromControl(formGroup: FormGroup): ApiResource {
        const result = new ApiResource();
        result.state = formGroup.dirty ? formGroup.get('state').value === EntityState.Unchanged ? EntityState.Modified : formGroup.get('state').value : formGroup.get('state').value;
        result.id = formGroup.get('id').value;
        result.enabled = formGroup.get('enabled').value;
        result.name = formGroup.get('name').value;
        result.displayName = formGroup.get('displayName').value;
        result.description = formGroup.get('description').value;
        result.created = formGroup.get('created').value;
        result.updated = formGroup.get('updated').value;
        result.lastAccessed = formGroup.get('lastAccessed').value;
        result.nonEditable = formGroup.get('nonEditable').value;
        result.secrets = [];
        result.scopes = [];
        result.userClaims = [];
        result.properties = [];
        (formGroup.get('secrets') as FormArray).controls.forEach(form => {
            result.secrets.push(ApiSecret.fromControl(form as FormGroup));
        });
        (formGroup.get('scopes') as FormArray).controls.forEach(form => {
            result.scopes.push(ApiScope.fromControl(form as FormGroup));
        });
        (formGroup.get('userClaims') as FormArray).controls.forEach(form => {
            result.userClaims.push(ApiResourceClaim.fromControl(form as FormGroup));
        });
        (formGroup.get('properties') as FormArray).controls.forEach(form => {
            result.properties.push(ApiResourceProperty.fromControl(form as FormGroup));
        });
        return result;
    }
}

abstract class Secret extends BaseModel<number> {
    description?: string;
    value: string;
    expiration?: Date;
    type: string;
    created: Date;

    constructor() {
        super();
    }

    static toControl(fb: FormBuilder, src: Secret): FormGroup {
        src.state = EntityState.Unchanged;
        const control = fb.group({
            state: [EntityState.Unchanged],
            id: [src.id, Validators.required],
            description: [src.description],
            value: [src.value, Validators.required],
            expiration: [src.expiration],
            type: [src.type, Validators.required],
            created: [src.created, Validators.required]
        });
        return control;
    }
}

export class ApiSecret extends Secret {
    apiResourceId: number;
    constructor() {
        super();
    }
    static toControl(fb: FormBuilder, src: ApiSecret): FormGroup {
        const control = super.toControl(fb, src);
        control.addControl('apiResourceId', fb.control(src.apiResourceId, Validators.required));
        return control;
    }

    static fromControl(formGroup: FormGroup): ApiSecret {
        const result = new ApiSecret();
        result.state = formGroup.dirty ? formGroup.get('state').value === EntityState.Unchanged ? EntityState.Modified : formGroup.get('state').value : formGroup.get('state').value;
        result.id = formGroup.get('id').value;
        result.description = formGroup.get('description').value;
        result.value = formGroup.get('value').value;
        result.expiration = formGroup.get('expiration').value;
        result.type = formGroup.get('type').value;
        result.created = formGroup.get('created').value;
        result.apiResourceId = formGroup.get('apiResourceId').value;
        return result;
    }
}

abstract class UserClaim extends BaseModel<number> {
    type: string;
    constructor() {
        super();
    }

    static toControl(fb: FormBuilder, src: UserClaim): FormGroup {
        src.state = EntityState.Unchanged;
        const control = fb.group({
            state: [EntityState.Unchanged],
            id: [src.id, Validators.required],
            type: [src.type, Validators.required]
        });
        return control;
    }
}

export class ApiScopeClaim extends UserClaim {
    apiScopeId: number;
    constructor() {
        super();
    }

    static toControl(fb: FormBuilder, src: ApiScopeClaim): FormGroup {
        const control = super.toControl(fb, src);
        control.addControl('apiScopeId', fb.control(src.apiScopeId, Validators.required));
        return control;
    }

    static fromControl(formGroup: FormGroup): ApiScopeClaim {
        const result = new ApiScopeClaim();
        result.state = formGroup.dirty ? formGroup.get('state').value === EntityState.Unchanged ? EntityState.Modified : formGroup.get('state').value : formGroup.get('state').value;
        result.id = formGroup.get('id').value;
        result.type = formGroup.get('type').value;
        result.apiScopeId = formGroup.get('apiScopeId').value;
        return result;
    }
}

export class ApiScope extends BaseModel<number> {
    name: string;
    displayName: string;
    description: string;
    required: boolean;
    emphasize: boolean;
    showInDiscoveryDocument: boolean;
    userClaims: ApiScopeClaim[] = [];
    apiResourceId: number;
    constructor() {
        super();
        this.id = 0;
        this.required = false;
        this.emphasize = false;
        this.showInDiscoveryDocument = false;
    }

    static toControl(fb: FormBuilder, src: ApiScope): FormGroup {
        src.state = EntityState.Unchanged;
        const control = fb.group({
            state: [EntityState.Unchanged],
            id: [src.id, Validators.required],
            name: [src.name, {
                validators: [Validators.required],
                updateOn: 'blur'
            }],
            displayName: [src.displayName],
            description: [src.description],
            required: [src.required, Validators.required],
            emphasize: [src.emphasize, Validators.required],
            showInDiscoveryDocument: [src.showInDiscoveryDocument],
            apiResourceId: [src.apiResourceId, Validators.required],
            userClaims: fb.array([]),
        });
        src.userClaims.forEach(item => {
            (control.get('userClaims') as FormArray).push(ApiScopeClaim.toControl(fb, item));
        });
        return control;
    }

    static fromControl(formGroup: FormGroup): ApiScope {
        const result = new ApiScope();
        result.state = formGroup.dirty ? formGroup.get('state').value === EntityState.Unchanged ? EntityState.Modified : formGroup.get('state').value : formGroup.get('state').value;
        result.id = formGroup.get('id').value;
        result.name = formGroup.get('name').value;
        result.displayName = formGroup.get('displayName').value;
        result.description = formGroup.get('description').value;
        result.required = formGroup.get('required').value;
        result.emphasize = formGroup.get('emphasize').value;
        result.showInDiscoveryDocument = formGroup.get('showInDiscoveryDocument').value;
        result.apiResourceId = formGroup.get('apiResourceId').value;
        result.userClaims = [];
        (formGroup.get('userClaims') as FormArray).controls.forEach(form => {
            result.userClaims.push(ApiScopeClaim.fromControl(form as FormGroup));
        });
        return result;
    }
}

export class ApiResourceClaim extends UserClaim {
    apiResourceId: number;
    constructor() {
        super();
    }

    static toControl(fb: FormBuilder, src: ApiResourceClaim): FormGroup {
        const control = super.toControl(fb, src);
        control.addControl('apiResourceId', fb.control(src.apiResourceId, Validators.required));
        return control;
    }

    static fromControl(formGroup: FormGroup): ApiResourceClaim {
        const result = new ApiResourceClaim();
        result.state = formGroup.dirty ? formGroup.get('state').value === EntityState.Unchanged ? EntityState.Modified : formGroup.get('state').value : formGroup.get('state').value;
        result.id = formGroup.get('id').value;
        result.type = formGroup.get('type').value;
        return result;
    }
}

abstract class Property extends BaseModel<number> {
    key: string;
    value: string;
    constructor() {
        super();
    }

    static toControl(fb: FormBuilder, src: Property): FormGroup {
        src.state = EntityState.Unchanged;
        const control = fb.group({
            state: [EntityState.Unchanged],
            id: [src.id, Validators.required],
            key: [src.key, Validators.required],
            value: [src.value, Validators.required]
        });
        return control;
    }
}

export class ApiResourceProperty extends Property {
    apiResourceId: number;
    constructor() {
        super();
    }

    static toControl(fb: FormBuilder, src: ApiResourceProperty): FormGroup {
        const control = super.toControl(fb, src);
        control.addControl('apiResourceId', fb.control(src.apiResourceId, Validators.required));
        return control;
    }

    static fromControl(formGroup: FormGroup): ApiResourceProperty {
        const result = new ApiResourceProperty();
        result.state = formGroup.dirty ? formGroup.get('state').value === EntityState.Unchanged ? EntityState.Modified : formGroup.get('state').value : formGroup.get('state').value;
        result.id = formGroup.get('id').value;
        result.key = formGroup.get('key').value;
        result.value = formGroup.get('value').value;
        return result;
    }
}
