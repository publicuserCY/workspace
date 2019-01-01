import { Component, OnInit } from '@angular/core';
import { AuthorityService } from '../services/authority.service';
import { ApiResource } from '../models/api-resource';
import { ApiResourceRequestModel } from '../models/request';
import { finalize, delay, map } from 'rxjs/operators';
import { NzMessageService } from 'ng-zorro-antd';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, FormControl, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UniqueApiResourceNameValidator } from '../validator/api-resource-name.validator';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-authority-api-resource-detail',
  templateUrl: './api-resource-detail.component.html',
  styleUrls: ['./api-resource-detail.component.css']
})
export class ApiResourceDetailComponent implements OnInit {
  // id$: Observable<number>;
  isSpinning = false;
  // entity: ApiResource;
  // entity$: Observable<ApiResource>;
  mainForm: FormGroup;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private nzMessageService: NzMessageService,
    private authorityService: AuthorityService,
    private uniqueApiResourceNameValidator: UniqueApiResourceNameValidator

  ) { }

  ngOnInit() {
    /* this.id$ = this.route.paramMap.pipe(
          map(params => +params.get('id'))
        ); */
    const id = +this.route.snapshot.paramMap.get('id');
    this.mainForm = this.fb.group({
      enabled: [true],
      name: ['',
        {
          validators: [Validators.required],
          asyncValidators: [this.uniqueApiResourceNameValidator.validate.bind(this.uniqueApiResourceNameValidator)],
          updateOn: 'blur'
        }],
      displayName: [''],
      description: [''],
      nonEditable: [false],
    });

    if (id !== 0) {
      const model = new ApiResourceRequestModel();
      model.id = id;
      this.authorityService.selectApiResource(model).pipe(
        finalize(() => { this.isSpinning = false; })
      ).subscribe(
        result => {
          if (result.isSuccess && result.data.length > 0) {
            const entity = result.data[0];
            this.mainForm.patchValue(
              {
                id: [entity.id],
                enabled: [entity.enabled],
                name: [entity.name],
                displayName: [entity.displayName],
                description: [entity.description],
                nonEditable: [entity.nonEditable],
              }
            );
          } else {
            this.nzMessageService.error(result.message);
          }
        }
      );
    }

  }

  submit() {
    const model = new ApiResourceRequestModel();
    model.enabled = this.mainForm.get('enabled').value;
    model.name = this.mainForm.get('name').value;
    model.displayName = this.mainForm.get('displayName').value;
    model.description = this.mainForm.get('description').value;
    model.nonEditable = this.mainForm.get('nonEditable').value;
    this.authorityService.insertApiResource(model).subscribe(result => {
      if (result.isSuccess) {
        this.goBack();
      } else {
        this.nzMessageService.error(result.message);
      }
    });
  }

  reset(e: MouseEvent): void {
    e.preventDefault();
    const initialMap = {
      enabled: true,
      name: '',
      displayName: '',
      description: '',
      nonEditable: false
    };
    this.mainForm.reset(initialMap);
    for (const key of Object.keys(this.mainForm.controls)) {
      this.mainForm.controls[key].markAsPristine();
      this.mainForm.controls[key].updateValueAndValidity();
    }
  }

  goBack() {
    this.router.navigate(['../ApiResources'], { relativeTo: this.route });
  }
}
