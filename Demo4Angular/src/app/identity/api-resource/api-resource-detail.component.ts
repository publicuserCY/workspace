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

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private nzMessageService: NzMessageService,
    private apiResourceService: ApiResourceService,
  ) { }

  ngOnInit() {
    this.mainForm = ApiResource.toControl(this.fb, this.initModel);
    this.mainForm.get('name').setAsyncValidators(uniqueApiResourceNameValidatorFn(this.apiResourceService, 0));
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
            Object.assign(this.initModel, result.data);
            this.mainForm = ApiResource.toControl(this.fb, this.initModel);
            this.mainForm.get('name').setAsyncValidators(uniqueApiResourceNameValidatorFn(this.apiResourceService, this.initModel.id));
            (this.mainForm.get('scopes') as FormArray).controls.forEach(scope => {
              (scope as FormGroup).get('name').setAsyncValidators(uniqueApiScopeNameValidatorFn(this.apiResourceService, (scope as FormGroup).get('id').value));
            });
            this.mainForm.reset(this.initModel);
          } else {
            this.nzMessageService.error(result.message);
          }
        }
      );
  }

  addApiScret(formGroup: FormGroup) {
    (formGroup.get('secrets') as FormArray).push(this.fb.group({
      state: [EntityState.Added],
      id: [0, Validators.required],
      description: [null],
      value: [null, Validators.required],
      expiration: [null],
      type: [null, Validators.required],
      created: [new Date(), Validators.required],
      apiResourceId: [formGroup.get('id').value, Validators.required]
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

  addApiScope(formGroup: FormGroup) {
    (formGroup.get('scopes') as FormArray).push(this.fb.group({
      state: [EntityState.Added],
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
      apiResourceId: [formGroup.get('id').value, Validators.required],
      userClaims: this.fb.array([])
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
      state: [EntityState.Added],
      id: [0, Validators.required],
      type: [null, Validators.required],
      apiScopeId: [formGroup.get('id').value, Validators.required]
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

  addApiResourceClaim(formGroup: FormGroup) {
    (formGroup.get('userClaims') as FormArray).push(this.fb.group({
      state: [EntityState.Added],
      id: [0, Validators.required],
      type: [null, Validators.required],
      apiResourceId: [formGroup.get('id').value, Validators.required]
    }));
  }

  deleteApiResourceClaim(formGroup: FormGroup, index: number) {
    if (formGroup.get('state').value === EntityState.Added) {
      (formGroup.parent as FormArray).removeAt(index);
    } else {
      formGroup.patchValue({ state: EntityState.Deleted });
      formGroup.markAsDirty();
    }
  }

  addApiResourceProperty(formGroup: FormGroup) {
    (formGroup.get('properties') as FormArray).push(this.fb.group({
      state: [EntityState.Added],
      id: [0, Validators.required],
      key: [null, Validators.required],
      value: [null, Validators.required],
      apiResourceId: [formGroup.get('id').value, Validators.required]
    }));
  }

  deleteApiRsourceProperty(formGroup: FormGroup, index: number) {
    if (formGroup.get('state').value === EntityState.Added) {
      (formGroup.parent as FormArray).removeAt(index);
    } else {
      formGroup.patchValue({ state: EntityState.Deleted });
      formGroup.markAsDirty();
    }
  }

  submit() {
    this.isSpinning = true;
    this.initModel = ApiResource.fromControl(this.mainForm);
    let requestModel: ApiResourceRequestModel;
    if (this.mainForm.get('state').value === EntityState.Added) {
      requestModel = new ApiResourceRequestModel(Uris.AddApiResource);
    } else {
      requestModel = new ApiResourceRequestModel(Uris.ModifyApiResource);
    }
    requestModel.apiResource = this.initModel;
    this.apiResourceService.submit(requestModel).pipe(
      finalize(() => this.isSpinning = false)
    ).subscribe(
      result => {
        if (result.isSuccess) {
          if (this.mainForm.get('state').value === EntityState.Added) {
            this.nzMessageService.info('ApiResource 新增完成');
            this.router.navigate(['../', result.data.id], { relativeTo: this.route });
          } else {
            Object.assign(this.initModel, result.data);
            this.mainForm = ApiResource.toControl(this.fb, this.initModel);
            this.mainForm.get('name').setAsyncValidators(uniqueApiResourceNameValidatorFn(this.apiResourceService, this.initModel.id));
            (this.mainForm.get('scopes') as FormArray).controls.forEach(scope => {
              (scope as FormGroup).get('name').setAsyncValidators(uniqueApiScopeNameValidatorFn(this.apiResourceService, (scope as FormGroup).get('id').value));
            });
            this.mainForm.reset(this.initModel);
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
