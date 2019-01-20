import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router, NavigationExtras } from '@angular/router';
import { Subscription, of, empty } from 'rxjs';
import { finalize, switchMap } from 'rxjs/operators';
import { NzMessageService } from 'ng-zorro-antd';

import { ApiResourceRequestModel } from '../model/api-resource-request.model';
import { ApiResource, ApiSecret, ApiScope } from '../model/api-resource.model';
import { ApiResourceService } from '../service/api-resource.service';
import { AuthorityInteractionService } from '../service/authority-Interaction.service';
import { uniqueApiResourceNameValidatorFn } from '../validator/api-resource-name.validator';
import { EntityState, Uris } from 'src/app/shared/const';
import { OperationResult, PaginatedResult } from 'src/app/shared/result';
// import * as fns from 'date-fns';
@Component({
  selector: 'app-authority-api-resource-detail',
  templateUrl: './api-resource-detail.component.html',
  styleUrls: ['./api-resource-detail.component.css']
})
export class ApiResourceDetailComponent implements OnInit, OnDestroy {
  isSpinning = false;
  apiResource = new ApiResource();
  mainForm: FormGroup;
  apiSecretSubscription: Subscription;
  apiScopeSubscription: Subscription;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private nzMessageService: NzMessageService,
    private apiResourceService: ApiResourceService,
    private authorityInteractionService: AuthorityInteractionService
  ) { }

  ngOnInit() {
    this.mainForm = this.fb.group({
      id: [this.apiResource.id],
      enabled: [this.apiResource.enabled],
      name: [this.apiResource.name,
      {
        validators: [Validators.required],
        asyncValidators: [uniqueApiResourceNameValidatorFn(this.apiResourceService, this.apiResource.id)],
        updateOn: 'blur'
      }],
      displayName: [this.apiResource.displayName],
      description: [this.apiResource.description],
      nonEditable: [this.apiResource.nonEditable],
      created: [this.apiResource.created]
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
            this.apiResource.state = EntityState.Added;
            const v: OperationResult<ApiResource> = { isSuccess: true, data: null };
            return of(v);
          }
        })
      ).subscribe(
        result => {
          if (result.isSuccess) {
            if (result.data) {
              ApiResource.assign(this.apiResource, result.data);
              this.mainForm.get('name').setAsyncValidators(uniqueApiResourceNameValidatorFn(this.apiResourceService, this.apiResource.id));
              this.reset();
            }
          } else {
            this.nzMessageService.error(result.message);
          }
        }
      );
    this.apiSecretSubscription = this.authorityInteractionService.apiSecret$.subscribe(
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
      });
  }

  addApiScret() {
    const secret = new ApiSecret();
    this.apiResource.addSecret(secret);
  }

  addApiScope() {
    const apiScope = new ApiScope();
    this.apiResource.addScope(apiScope);
  }

  submit() {
    this.isSpinning = true;

    this.apiResource.enabled = this.mainForm.get('enabled').value;
    this.apiResource.name = this.mainForm.get('name').value;
    this.apiResource.displayName = this.mainForm.get('displayName').value;
    this.apiResource.description = this.mainForm.get('description').value;
    this.apiResource.nonEditable = this.mainForm.get('nonEditable').value;

    let requestModel: ApiResourceRequestModel;
    if (this.apiResource.state === EntityState.Added) {
      requestModel = new ApiResourceRequestModel(Uris.AddApiResource);
    } else {
      requestModel = new ApiResourceRequestModel(Uris.ModifyApiResource);
    }
    requestModel.apiResource = this.apiResource;
    this.apiResourceService.submit(requestModel).pipe(
      finalize(() => this.isSpinning = false)
    ).subscribe(
      result => {
        if (result.isSuccess) {
          if (this.apiResource.state === EntityState.Added) {
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

  reset(): void {
    let initialMap = {};
    if (this.apiResource.state === EntityState.Added) {
      initialMap = {
        id: null,
        enabled: true,
        name: null,
        displayName: null,
        description: null,
        nonEditable: false,
        created: null
      };
    } else {
      initialMap = {
        id: this.apiResource.id,
        enabled: this.apiResource.enabled,
        name: this.apiResource.name,
        displayName: this.apiResource.displayName,
        description: this.apiResource.description,
        nonEditable: this.apiResource.nonEditable,
        created: this.apiResource.created
      };
    }
    this.mainForm.reset(initialMap);
    /* for (const key of Object.keys(this.mainForm.controls)) {
      this.mainForm.controls[key].markAsPristine();
      this.mainForm.controls[key].updateValueAndValidity();
    } */
  }

  goBack() {
    const navigationExtras: NavigationExtras = {
      relativeTo: this.route,
      queryParamsHandling: 'preserve'
    };
    this.router.navigate(['../', { id: this.apiResource.id }], navigationExtras);
  }

  ngOnDestroy(): void {
    this.apiSecretSubscription.unsubscribe();
  }
}
