import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, Observable, of } from 'rxjs';
import { finalize, delay, map, switchMap } from 'rxjs/operators';
import { NzMessageService } from 'ng-zorro-antd';

import { ApiResourceRequestModel, ApiSecretRequestModel } from '../models/api-resource-request.model';
import { ApiResource, ApiSecret } from '../models/api-resource.model';
import { AuthorityService } from '../services/authority.service';
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
  apiSecretAdded: Subscription;
  apiSecretModified: Subscription;
  apiSecretDeleted: Subscription;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private nzMessageService: NzMessageService,
    private authorityService: AuthorityService,
    private authorityInteractionService: AuthorityInteractionService
  ) { }

  ngOnInit() {
    this.route.paramMap
      .pipe(
        switchMap((params) => {
          const id = +params.get('id');
          if (id > 0) {
            const requestModel = new ApiResourceRequestModel();
            requestModel.id = id;
            return this.authorityService.retrieveApiResource(requestModel).pipe(
              finalize(() => { this.isSpinning = false; })
            );
          } else {
            const v: OperationResult<PaginatedResult<ApiResource>> = { isSuccess: true, data: new PaginatedResult() };
            return of(v);
          }
        })
      ).subscribe(
        result => {
          if (result.isSuccess) {
            if (result.data.list.length > 0) {
              Object.assign(this.apiResource, result.data.list[0]);
              this.apiResource.state = EntityState.Modified;
              const initialMap = {
                id: this.apiResource.id,
                enabled: this.apiResource.enabled,
                name: this.apiResource.name,
                displayName: this.apiResource.displayName,
                description: this.apiResource.description,
                nonEditable: this.apiResource.nonEditable
              };
              this.mainForm.get('name').setAsyncValidators(uniqueApiResourceNameValidatorFn(this.authorityService, this.apiResource.id));
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
        asyncValidators: [uniqueApiResourceNameValidatorFn(this.authorityService, this.apiResource.id)],
        updateOn: 'blur'
      }],
      displayName: [this.apiResource.displayName],
      description: [this.apiResource.description],
      nonEditable: [this.apiResource.nonEditable],
    });
    this.apiSecretAdded = this.authorityInteractionService.apiSecretAdded$.subscribe(
      item => {
        this.apiResource.secrets = [...this.apiResource.secrets, item];
      });
    this.apiSecretModified = this.authorityInteractionService.apiSecretModified$.subscribe(
      item => {
        const index = this.apiResource.secrets.findIndex(p => p.id === item.id);
        Object.assign(this.apiResource.secrets[index], item);
      });
    this.apiSecretDeleted = this.authorityInteractionService.apiSecretDeleted$.subscribe(
      item => {
        this.apiResource.secrets = this.apiResource.secrets.filter(p => p.id !== item.id);
      });
  }

  addApiScret() {
    const secret = new ApiSecret();
    this.apiResource.addApiSecret(secret);
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
    if (this.apiResource.state === EntityState.Added) {
      this.authorityService.addApiResource(requestModel).pipe(
        finalize(() => this.isSpinning = false)
      ).subscribe(
        result => {
          if (result.isSuccess) {
            /* Object.assign(this.apiResource, result.data);
            this.apiResource.state = EntityState.Modified; */
            this.nzMessageService.info('ApiResource 新增完成');
            this.router.navigate(['../', result.data.id], { relativeTo: this.route });
          } else {
            this.nzMessageService.error(result.message);
          }
        }
      );
    } else {
      this.authorityService.modifyApiResource(requestModel).pipe(
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
    }
  }

  reset(): void {
    // e.preventDefault();
    let initialMap = {};
    if (this.apiResource.state === EntityState.Added) {
      initialMap = {
        enabled: true,
        name: '',
        displayName: '',
        description: '',
        nonEditable: false
      };
    } else {
      initialMap = {
        id: this.apiResource.id,
        enabled: this.apiResource.enabled,
        name: this.apiResource.name,
        displayName: this.apiResource.displayName,
        description: this.apiResource.description,
        nonEditable: this.apiResource.nonEditable
      };
    }
    this.mainForm.reset(initialMap);
    /* for (const key of Object.keys(this.mainForm.controls)) {
      this.mainForm.controls[key].markAsPristine();
      this.mainForm.controls[key].updateValueAndValidity();
    } */
  }

  goBack() {
    this.router.navigate(['../', { id: this.apiResource.id }], { relativeTo: this.route });
  }

  ngOnDestroy(): void {
    this.apiSecretAdded.unsubscribe();
    this.apiSecretModified.unsubscribe();
    this.apiSecretDeleted.unsubscribe();
  }
}
