import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router, NavigationExtras } from '@angular/router';
import { Subscription, of } from 'rxjs';
import { finalize, switchMap } from 'rxjs/operators';
import { NzMessageService } from 'ng-zorro-antd';

import { ApiResourceRequestModel } from '../models/api-resource-request.model';
import { ApiResource, ApiSecret, ApiScope } from '../models/api-resource.model';
import { ApiResourceService } from '../services/api-resource.service';
import { AuthorityInteractionService } from '../services/authority-Interaction.service';
import { uniqueApiResourceNameValidatorFn } from '../validator/api-resource-name.validator';
import { EntityState } from 'src/app/shared/const';
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
    this.route.paramMap
      .pipe(
        switchMap((params) => {
          const id = params.get('id');
          if (id) {
            this.apiResource.state = EntityState.Modified;
            const requestModel = new ApiResourceRequestModel();
            requestModel.criteria = id;
            return this.apiResourceService.single(requestModel).pipe(
              finalize(() => { this.isSpinning = false; })
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
              Object.assign(this.apiResource, result.data);
              const initialMap = {
                id: this.apiResource.id,
                enabled: this.apiResource.enabled,
                name: this.apiResource.name,
                displayName: this.apiResource.displayName,
                description: this.apiResource.description,
                nonEditable: this.apiResource.nonEditable,
                created: this.apiResource.created
              };
              this.mainForm.get('name').setAsyncValidators(uniqueApiResourceNameValidatorFn(this.apiResourceService, this.apiResource.id));
              this.mainForm.reset(initialMap);
            }
          } else {
            this.nzMessageService.error(result.message);
          }
        }
      );
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
    this.apiSecretSubscription = this.authorityInteractionService.apiSecret$.subscribe(
      item => {
        switch (item.state) {
          case EntityState.Added:
            break;
          case EntityState.Modified:
            this.apiResource.modifyApiSecret(item);
            break;
          case EntityState.Deleted:
            this.apiResource.deleteApiSecret(item);
            break;
        }
      });
    this.apiScopeSubscription = this.authorityInteractionService.apiScope$.subscribe(
      item => {
        switch (item.state) {
          case EntityState.Added:
            break;
          case EntityState.Modified:
            this.apiResource.modifyApiScope(item);
            break;
          case EntityState.Deleted:
            this.apiResource.deleteApiScope(item);
            break;
        }
      });
  }

  addApiScret() {
    const secret = new ApiSecret();
    this.apiResource.addApiSecret(secret);
  }

  addApiScope() {
    const apiScope = new ApiScope();
    this.apiResource.addApiScope(apiScope);
  }

  submit() {
    this.isSpinning = true;

    this.apiResource.enabled = this.mainForm.get('enabled').value;
    this.apiResource.name = this.mainForm.get('name').value;
    this.apiResource.displayName = this.mainForm.get('displayName').value;
    this.apiResource.description = this.mainForm.get('description').value;
    this.apiResource.nonEditable = this.mainForm.get('nonEditable').value;

    const requestModel = new ApiResourceRequestModel();
    requestModel.apiResource = this.apiResource;
    this.apiResourceService.submit(requestModel).pipe(
      finalize(() => this.isSpinning = false)
    ).subscribe(
      result => {
        if (result.isSuccess) {
          if (this.apiResource.state === EntityState.Added) {
            this.nzMessageService.info('ApiResource 新增完成');
            this.router.navigate(['../', result.data.id], { relativeTo: this.route });
          } else if (this.apiResource.state === EntityState.Modified) {
            this.nzMessageService.info('ApiResource 更新完成');
          }
        } else {
          this.nzMessageService.error(result.message);
        }
      }
    );
    /* if (this.apiResource.state === EntityState.Added) {
      this.apiResourceService.add(requestModel).pipe(
        finalize(() => this.isSpinning = false)
      ).subscribe(
        result => {
          if (result.isSuccess) {
            this.nzMessageService.info('ApiResource 新增完成');
            this.router.navigate(['../', result.data.id], { relativeTo: this.route });
          } else {
            this.nzMessageService.error(result.message);
          }
        }
      );
    } else {
      this.apiResourceService.modify(requestModel).pipe(
        finalize(() => this.isSpinning = false)
      ).subscribe(
        result => {
          if (result.isSuccess) {
            this.nzMessageService.info('ApiResource 更新完成');
          } else {
            this.nzMessageService.error(result.message);
          }
        }
      );
    } */
  }

  reset(): void {
    let initialMap = {};
    if (this.apiResource.state === EntityState.Added) {
      initialMap = {
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
    this.router.navigate(['../', { id: this.apiResource.id  }], navigationExtras);
  }

  ngOnDestroy(): void {
    this.apiSecretSubscription.unsubscribe();
  }
}
