import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { finalize, delay, map } from 'rxjs/operators';
import { NzMessageService } from 'ng-zorro-antd';

import { ApiResourceRequestModel } from '../models/api-resource-request.model';
import { ApiResource, ApiSecret } from '../models/api-resource.model';
import { AuthorityService } from '../services/authority.service';
import { AuthorityInteractionService } from '../services/authority-Interaction.service';
import { uniqueApiResourceNameValidatorFn } from '../validator/api-resource-name.validator';
import { EntityState } from 'src/app/shared/const';


// import * as fns from 'date-fns';

@Component({
  selector: 'app-authority-api-resource-detail',
  templateUrl: './api-resource-detail.component.html',
  styleUrls: ['./api-resource-detail.component.css']
})
export class ApiResourceDetailComponent implements OnInit, OnDestroy {
  // id$: Observable<number>;
  isSpinning = false;
  apiResource = new ApiResource();
  // entity$: Observable<ApiResource>;
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

    /* this.id$ = this.route.paramMap.pipe(
          map(params => +params.get('id'))
        ); */
    this.apiResource.id = +this.route.snapshot.paramMap.get('id');
    this.mainForm = this.fb.group({
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

    if (this.apiResource.id !== 0) {
      const requestModel = new ApiResourceRequestModel();
      requestModel.id = this.apiResource.id;
      this.authorityService.selectApiResource(requestModel).pipe(
        finalize(() => { this.isSpinning = false; })
      ).subscribe(
        result => {
          if (result.isSuccess && result.data.length > 0) {
            this.apiResource = result.data[0];
            this.apiResource.state = EntityState.Modified;
            this.mainForm.patchValue(
              {
                enabled: this.apiResource.enabled,
                name: this.apiResource.name,
                displayName: this.apiResource.displayName,
                description: this.apiResource.description,
                nonEditable: this.apiResource.nonEditable
              }
            );
          } else {
            this.nzMessageService.error(result.message);
          }
        }
      );
    }
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
    // this.updateSecretsEditCache();
  }

  /*   updateSecretsEditCache() {
      this.entity.secrets.forEach(item => {
        if (!this.secretsEditCache[item.id]) {
          this.secretsEditCache[item.id] = {
            edit: false,
            description: item.description,
            value: item.value,
            expiration: item.expiration,
            type: item.type,
            created: item.created
          };
        }
      });
    } */

  /*   startEditSecret(id: number) { this.secretsEditCache[id].edit = true; }
    cancelEditSecret(id: number) { this.secretsEditCache[id].edit = false; }
    saveSecret(id: number) {
      const index = this.entity.secrets.findIndex(item => item.id === id);
      Object.assign(this.entity.secrets[index], this.secretsEditCache[id]);
      this.entity.secrets[index].flag = this.entity.secrets[index].flag === Operational.Origin ? Operational.Update : this.entity.secrets[index].flag;
      this.secretsEditCache[id].edit = false;
    }
    deleteSecret(id: number) {
      const index = this.entity.secrets.findIndex(item => item.id === id);
      this.entity.secrets[index].flag = Operational.Delete;
      this.entity.secrets = this.entity.secrets.filter(p => p.flag !== Operational.Delete);
    } */

  submit() {
    this.isSpinning = true;

    this.apiResource.enabled = this.mainForm.get('enabled').value;
    this.apiResource.name = this.mainForm.get('name').value;
    this.apiResource.displayName = this.mainForm.get('displayName').value;
    this.apiResource.description = this.mainForm.get('description').value;
    this.apiResource.nonEditable = this.mainForm.get('nonEditable').value;
    this.apiResource.secrets = this.apiResource.secrets;

    const requestModel = new ApiResourceRequestModel();
    requestModel.apiResource = this.apiResource;
    if (this.apiResource.state === EntityState.Added) {
      this.authorityService.insertApiResource(requestModel).pipe(
        finalize(() => this.isSpinning = false)
      ).subscribe(
        result => {
          if (result.isSuccess) {
            this.apiResource = result.data[0];
            this.apiResource.state = EntityState.Modified;
            this.nzMessageService.info('新增完成');
          } else {
            this.nzMessageService.error(result.message);
          }
        }
      );
    } else {
      this.authorityService.updateApiResource(requestModel).pipe(
        finalize(() => this.isSpinning = false)
      ).subscribe(
        result => {
          if (result.isSuccess) {
            this.nzMessageService.info('更新完成');
          } else {
            this.nzMessageService.error(result.message);
          }
        }
      );
    }
  }

  reset(e: MouseEvent): void {
    e.preventDefault();
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
        enabled: this.apiResource.enabled,
        name: this.apiResource.name,
        displayName: this.apiResource.displayName,
        description: this.apiResource.description,
        nonEditable: this.apiResource.nonEditable
      };
    }
    this.mainForm.reset(initialMap);
    for (const key of Object.keys(this.mainForm.controls)) {
      this.mainForm.controls[key].markAsPristine();
      this.mainForm.controls[key].updateValueAndValidity();
    }
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
