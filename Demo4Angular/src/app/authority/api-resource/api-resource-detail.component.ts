import { Component, OnInit } from '@angular/core';
import { AuthorityService } from '../services/authority.service';
import { ApiResource, ApiSecret, Operational } from '../models/api-resource';
import { ApiResourceRequestModel } from '../models/request';
import { finalize, delay, map } from 'rxjs/operators';
import { NzMessageService } from 'ng-zorro-antd';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, FormControl, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { uniqueApiResourceNameValidatorFn } from '../validator/api-resource-name.validator';
import { Observable } from 'rxjs';
// import * as fns from 'date-fns';

@Component({
  selector: 'app-authority-api-resource-detail',
  templateUrl: './api-resource-detail.component.html',
  styleUrls: ['./api-resource-detail.component.css']
})
export class ApiResourceDetailComponent implements OnInit {
  // id$: Observable<number>;
  isSpinning = false;
  entity = new ApiResource();
  // entity$: Observable<ApiResource>;
  mainForm: FormGroup;
  secretsEditCache = {};

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private nzMessageService: NzMessageService,
    private authorityService: AuthorityService
  ) { }

  ngOnInit() {

    /* this.id$ = this.route.paramMap.pipe(
          map(params => +params.get('id'))
        ); */
    this.entity.id = +this.route.snapshot.paramMap.get('id');
    this.mainForm = this.fb.group({
      enabled: [this.entity.enabled],
      name: [this.entity.name,
      {
        validators: [Validators.required],
        asyncValidators: [uniqueApiResourceNameValidatorFn(this.authorityService, this.entity.id)],
        updateOn: 'blur'
      }],
      displayName: [this.entity.displayName],
      description: [this.entity.description],
      nonEditable: [this.entity.nonEditable],
    });

    if (this.entity.id !== 0) {
      const model = new ApiResourceRequestModel();
      model.id = this.entity.id;
      this.authorityService.selectApiResource(model).pipe(
        finalize(() => { this.isSpinning = false; })
      ).subscribe(
        result => {
          if (result.isSuccess && result.data.length > 0) {
            this.entity = result.data[0];
            this.mainForm.patchValue(
              {
                enabled: this.entity.enabled,
                name: this.entity.name,
                displayName: this.entity.displayName,
                description: this.entity.description,
                nonEditable: this.entity.nonEditable
              }
            );
          } else {
            this.nzMessageService.error(result.message);
          }
        }
      );
    }
  }

  addApiScret() {
    const secret = new ApiSecret(this.entity, Operational.Insert);
    const sorted = this.entity.secrets.sort((a, b) => b.id - a.id);
    if (sorted && sorted.length > 0) {
      secret.id = sorted[0].id + 1;
    }
    this.entity.secrets = [...this.entity.secrets, secret];
    this.updateSecretsEditCache();
  }

  updateSecretsEditCache() {
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
  }

  startEditSecret(id: number) { this.secretsEditCache[id].edit = true; }
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
  }

  submit() {
    this.isSpinning = true;
    const model = new ApiResourceRequestModel();
    model.id = this.entity.id;
    model.enabled = this.mainForm.get('enabled').value;
    model.name = this.mainForm.get('name').value;
    model.displayName = this.mainForm.get('displayName').value;
    model.description = this.mainForm.get('description').value;
    model.nonEditable = this.mainForm.get('nonEditable').value;
    if (this.entity.id === 0) {
      this.authorityService.insertApiResource(model).pipe(
        finalize(() => this.isSpinning = false)
      ).subscribe(
        result => {
          if (result.isSuccess) {
            this.goBack();
          } else {
            this.nzMessageService.error(result.message);
          }
        }
      );
    } else {
      this.authorityService.updateApiResource(model).pipe(
        finalize(() => this.isSpinning = false)
      ).subscribe(
        result => {
          if (result.isSuccess) {
            this.goBack();
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
    if (this.entity.id === 0) {
      initialMap = {
        enabled: true,
        name: '',
        displayName: '',
        description: '',
        nonEditable: false
      };

    } else {
      initialMap = {
        enabled: this.entity.enabled,
        name: this.entity.name,
        displayName: this.entity.displayName,
        description: this.entity.description,
        nonEditable: this.entity.nonEditable
      };
    }
    this.mainForm.reset(initialMap);
    for (const key of Object.keys(this.mainForm.controls)) {
      this.mainForm.controls[key].markAsPristine();
      this.mainForm.controls[key].updateValueAndValidity();
    }
  }

  goBack() {
    this.router.navigate(['../'], { relativeTo: this.route });
  }
}
