import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute, Router, NavigationExtras } from '@angular/router';
import { Subscription, of, empty } from 'rxjs';
import { finalize, switchMap } from 'rxjs/operators';
import { NzMessageService } from 'ng-zorro-antd';

import { ApiResourceRequestModel } from '../model/api-resource-request.model';
import { ApiResource, ApiSecret, ApiScope, ApiScopeClaim, ApiResourceClaim, ApiResourceProperty } from '../model/api-resource.model';
import { ApiResourceService } from '../service/api-resource.service';
import { AuthorityInteractionService } from '../service/authority-Interaction.service';
import { uniqueApiResourceNameValidatorFn } from '../validator/api-resource-name.validator';
import { uniqueApiScopeNameValidatorFn } from '../validator/api-scope-name.validator';
import { EntityState, Uris } from 'src/app/shared/const';
import { OperationResult } from 'src/app/shared/result';
// import * as fns from 'date-fns';
@Component({
  selector: 'app-authority-api-resource-detail',
  templateUrl: './api-resource-detail.component.html',
  styleUrls: ['./api-resource-detail.component.css']
})
export class ApiResourceDetailComponent implements OnInit, OnDestroy {
  isSpinning = false;
  isEdit = false;
  isConfirmCancelVisible = false;
  mainForm: FormGroup;
  mainFormSpan = 5;
  labelSpan = 6;
  controlSpan = 18;
  initModel = new ApiResource();
  /* apiSecretSubscription: Subscription;
  apiScopeSubscription: Subscription; */

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private nzMessageService: NzMessageService,
    private apiResourceService: ApiResourceService,
  ) { }

  ngOnInit() {
    this.mainForm = this.fb.group({
      id: [null, Validators.required],
      enabled: [null, Validators.required],
      name: [null, { validators: [Validators.required], updateOn: 'blur' }],
      displayName: [null],
      description: [null],
      secrets: this.fb.array([]),
      scopes: this.fb.array([]),
      userClaims: this.fb.array([]),
      properties: this.fb.array([]),
      created: [null, Validators.required],
      updated: [null],
      lastAccessed: [null],
      nonEditable: [null, Validators.required],
      state: [null]
    });
    this.route.queryParamMap.subscribe(queryParams => {
      this.isEdit = queryParams.get('isEdit') === 'true';
    });
    this.route.paramMap
      .pipe(
        switchMap((params) => {
          const id = params.get('id');
          if (id) {
            const requestModel = new ApiResourceRequestModel(Uris.SingleApiResource);
            requestModel.criteria = id;
            return this.apiResourceService.single(requestModel).pipe(
              finalize(() => this.isSpinning = false)
            );
          } else {
            const apiResource = new ApiResource();
            apiResource.id = 0;
            apiResource.enabled = true;
            apiResource.created = new Date();
            apiResource.nonEditable = false;
            apiResource.state = EntityState.Added;
            const v: OperationResult<ApiResource> = { isSuccess: true, data: apiResource };
            return of(v);
          }
        })
      ).subscribe(
        result => {
          if (result.isSuccess) {
            this.mainForm = this.fb.group({
              id: [null, Validators.required],
              enabled: [null, Validators.required],
              name: [null, { validators: [Validators.required], updateOn: 'blur' }],
              displayName: [null],
              description: [null],
              secrets: this.fb.array([]),
              scopes: this.fb.array([]),
              userClaims: this.fb.array([]),
              properties: this.fb.array([]),
              created: [null, Validators.required],
              updated: [null],
              lastAccessed: [null],
              nonEditable: [null, Validators.required],
              state: [null]
            });
            result.data.secrets.forEach(secret => {
              (this.mainForm.get('secrets') as FormArray).push(this.fb.group({
                id: [secret.id, Validators.required],
                description: [secret.description],
                value: [secret.value, Validators.required],
                expiration: [secret.expiration],
                type: [secret.type, Validators.required],
                created: [secret.created, Validators.required],
                apiResourceId: [secret.apiResourceId, Validators.required],
                state: [EntityState.Unchanged]
              }));
            });
            result.data.scopes.forEach(scope => {
              const scopeGroup = this.fb.group({
                id: [scope.id, Validators.required],
                name: [scope.name, {
                  validators: [Validators.required],
                  asyncValidators: [uniqueApiScopeNameValidatorFn(this.apiResourceService, scope.id)],
                  updateOn: 'blur'
                }],
                displayName: [scope.displayName],
                description: [scope.description],
                required: [scope.required, Validators.required],
                emphasize: [scope.emphasize, Validators.required],
                showInDiscoveryDocument: [scope.showInDiscoveryDocument],
                apiResourceId: [scope.apiResourceId, Validators.required],
                userClaims: this.fb.array([]),
                state: [EntityState.Unchanged]
              });
              scope.userClaims.forEach(claim => {
                (scopeGroup.get('userClaims') as FormArray).push(this.fb.group({
                  id: [claim.id, Validators.required],
                  type: [claim.type, Validators.required],
                  apiScopeId: [claim.apiScopeId, Validators.required],
                  state: [EntityState.Unchanged]
                }));
              });
              (this.mainForm.get('scopes') as FormArray).push(scopeGroup);
            });
            result.data.userClaims.forEach(claim => {
              (this.mainForm.get('userClaims') as FormArray).push(this.fb.group({
                id: [claim.id, Validators.required],
                type: [claim.type, Validators.required],
                apiResourceId: [claim.apiResourceId, Validators.required],
                state: [EntityState.Unchanged]
              }));
            });
            result.data.properties.forEach(property => {
              (this.mainForm.get('properties') as FormArray).push(this.fb.group({
                id: [property.id, Validators.required],
                key: [property.key, Validators.required],
                value: [property.value, Validators.required],
                apiResourceId: [property.apiResourceId, Validators.required],
                state: [EntityState.Unchanged]
              }));
            });
            this.mainForm.get('name').setAsyncValidators(uniqueApiResourceNameValidatorFn(this.apiResourceService, result.data.id));
            Object.assign(this.initModel, result.data);
            this.mainForm.reset(this.initModel);
          } else {
            this.nzMessageService.error(result.message);
          }
        }
      );
    /* this.apiSecretSubscription = this.authorityInteractionService.apiSecret$.subscribe(
      item => {
        switch (item.state) {
          case EntityState.Added:
            break;
          case EntityState.Modified:
            this.apiResource.modifySecret(item);
            break;
          case EntityState.Deleted:
            this.apiResource.deleteSecret(item);
            break;
        }
      });
    this.apiScopeSubscription = this.authorityInteractionService.apiScope$.subscribe(
      item => {
        switch (item.state) {
          case EntityState.Added:
            break;
          case EntityState.Modified:
            this.apiResource.modifyScope(item);
            break;
          case EntityState.Deleted:
            this.apiResource.deleteScope(item);
            break;
        }
      }); */
  }

  addApiScret() {
    (this.mainForm.get('secrets') as FormArray).push(this.fb.group({
      id: [0, Validators.required],
      description: [null],
      value: [null, Validators.required],
      expiration: [null],
      type: [null, Validators.required],
      created: [new Date(), Validators.required],
      apiResourceId: [this.mainForm.get('id').value, Validators.required],
      state: [EntityState.Added]
    }));
  }

  deleteApiSecret(formGroup: FormGroup, index: number) {
    if (formGroup.get('state').value === EntityState.Added) {
      (formGroup.parent as FormArray).removeAt(index);
    } else {
      formGroup.patchValue({ state: EntityState.Deleted });
      formGroup.markAsDirty();
    }
  }

  addApiScope() {
    (this.mainForm.get('scopes') as FormArray).push(this.fb.group({
      id: [0, Validators.required],
      name: [null, {
        validators: [Validators.required],
        asyncValidators: [uniqueApiScopeNameValidatorFn(this.apiResourceService, 0)],
        updateOn: 'blur'
      }],
      displayName: [null],
      description: [null],
      required: [false, Validators.required],
      emphasize: [false, Validators.required],
      showInDiscoveryDocument: [false],
      apiResourceId: [this.mainForm.get('id').value, Validators.required],
      userClaims: this.fb.array([]),
      state: [EntityState.Added]
    }));
  }

  deleteApiScope(formGroup: FormGroup, index: number) {
    if (formGroup.get('state').value === EntityState.Added) {
      (formGroup.parent as FormArray).removeAt(index);
    } else {
      formGroup.patchValue({ state: EntityState.Deleted });
      formGroup.markAsDirty();
    }
  }

  addApiScopeClaim(formGroup: FormGroup) {
    (formGroup.get('userClaims') as FormArray).push(this.fb.group({
      id: [0, Validators.required],
      type: [null, Validators.required],
      state: [EntityState.Added]
    }));
  }

  deleteApiScopeClaim(formGroup: FormGroup, index: number) {
    if (formGroup.get('state').value === EntityState.Added) {
      (formGroup.parent as FormArray).removeAt(index);
    } else {
      formGroup.patchValue({ state: EntityState.Deleted });
      formGroup.markAsDirty();
    }
  }

  submit() {
    this.isSpinning = true;
    const apiResource = new ApiResource();
    apiResource.id = this.mainForm.get('id').value;
    apiResource.enabled = this.mainForm.get('enabled').value;
    apiResource.name = this.mainForm.get('name').value;
    apiResource.displayName = this.mainForm.get('displayName').value;
    apiResource.description = this.mainForm.get('description').value;
    apiResource.created = this.mainForm.get('created').value;
    apiResource.updated = this.mainForm.get('updated').value;
    apiResource.lastAccessed = this.mainForm.get('lastAccessed').value;
    apiResource.nonEditable = this.mainForm.get('nonEditable').value;
    (this.mainForm.get('secrets') as FormArray).controls.forEach(secret => {
      if (secret.dirty) {
        const apisecret = new ApiSecret();
        apisecret.id = secret.get('id').value;
        apisecret.description = secret.get('description').value;
        apisecret.value = secret.get('value').value;
        apisecret.expiration = secret.get('expiration').value;
        apisecret.type = secret.get('type').value;
        apisecret.created = secret.get('created').value;
        apisecret.apiResourceId = apiResource.id;
        apisecret.state = secret.get('state').value === EntityState.Unchanged ? EntityState.Modified : secret.get('state').value;
        apiResource.secrets.push(apisecret);
      }
    });
    (this.mainForm.get('scopes') as FormArray).controls.forEach(scope => {
      if (scope.dirty) {
        const apiScope = new ApiScope();
        apiScope.id = scope.get('id').value;
        apiScope.name = scope.get('name').value;
        apiScope.displayName = scope.get('displayName').value;
        apiScope.description = scope.get('description').value;
        apiScope.required = scope.get('required').value;
        apiScope.emphasize = scope.get('emphasize').value;
        apiScope.showInDiscoveryDocument = scope.get('showInDiscoveryDocument').value;
        apiScope.apiResourceId = scope.get('apiResourceId').value;
        apiScope.state = scope.get('state').value === EntityState.Unchanged ? EntityState.Modified : scope.get('state').value;
        (scope.get('userClaims') as FormArray).controls.forEach(claim => {
          const apiScopeClaim = new ApiScopeClaim();
          apiScopeClaim.id = claim.get('id').value;
          apiScopeClaim.type = claim.get('type').value;
          apiScopeClaim.apiScopeId = apiScope.id;
          apiScopeClaim.state = scope.get('state').value === EntityState.Unchanged ? EntityState.Modified : claim.get('state').value;
          apiScope.userClaims.push(apiScopeClaim);
        });
        apiResource.scopes.push(apiScope);
        // 更新scope name校验函数
        scope.get('name').setAsyncValidators(uniqueApiScopeNameValidatorFn(this.apiResourceService, apiScope.id));
      }
    });
    (this.mainForm.get('userClaims') as FormArray).controls.forEach(userClaim => {
      if (userClaim.dirty) {
        const apiResourceClaim = new ApiResourceClaim();
        apiResourceClaim.id = userClaim.get('id').value;
        apiResourceClaim.type = userClaim.get('type').value;
        apiResourceClaim.apiResourceId = apiResource.id;
        apiResourceClaim.state = userClaim.get('state').value === EntityState.Unchanged ? EntityState.Modified : userClaim.get('state').value;
        apiResource.userClaims.push(apiResourceClaim);
      }
    });
    (this.mainForm.get('properties') as FormArray).controls.forEach(property => {
      if (property.dirty) {
        const apiResourceProperty = new ApiResourceProperty();
        apiResourceProperty.id = property.get('id').value;
        apiResourceProperty.key = property.get('key').value;
        apiResourceProperty.value = property.get('value').value;
        apiResourceProperty.apiResourceId = apiResource.id;
        apiResourceProperty.state = property.get('state').value === EntityState.Unchanged ? EntityState.Modified : property.get('state').value;
        apiResource.properties.push(apiResourceProperty);
      }
    });
    let requestModel: ApiResourceRequestModel;
    if (this.mainForm.get('state').value === EntityState.Added) {
      requestModel = new ApiResourceRequestModel(Uris.AddApiResource);
    } else {
      requestModel = new ApiResourceRequestModel(Uris.ModifyApiResource);
    }
    requestModel.apiResource = apiResource;
    this.apiResourceService.submit(requestModel).pipe(
      finalize(() => this.isSpinning = false)
    ).subscribe(
      result => {
        if (result.isSuccess) {
          if (this.mainForm.get('state').value === EntityState.Added) {
            this.nzMessageService.info('ApiResource 新增完成');
            this.router.navigate(['../', result.data.id], { relativeTo: this.route });
          } else {
            this.nzMessageService.info('ApiResource 更新完成');
          }
        } else {
          this.nzMessageService.error(result.message);
        }
      }
    );
  }

  cancel(): void {
    if (this.mainForm.dirty) {
      this.isConfirmCancelVisible = true;
    } else {
      this.isEdit = false;
    }
  }

  confirmCancel() {
    this.mainForm.reset(this.initModel);
    this.isConfirmCancelVisible = false;
    this.isEdit = false;
  }

  goBack() {
    const navigationExtras: NavigationExtras = {
      relativeTo: this.route,
      queryParamsHandling: 'preserve'
    };
    this.router.navigate(['../', { id: this.mainForm.get('id').value }], navigationExtras);
  }

  ngOnDestroy(): void {
    // this.apiSecretSubscription.unsubscribe();
  }
}
