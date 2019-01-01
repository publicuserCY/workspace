import { Component, OnInit } from '@angular/core';
import { AuthorityService } from '../services/authority.service';
import { ApiResource } from '../models/api-resource';
import { ApiResourceRequestModel } from '../models/request';
import { finalize, delay } from 'rxjs/operators';
import { NzMessageService } from 'ng-zorro-antd';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-authority-api-resource',
  templateUrl: './api-resource.component.html',
  styleUrls: ['./api-resource.component.css']
})
export class ApiResourceComponent implements OnInit {
  isSpinning = false;
  apiResources: ApiResource[] = [];
  searchForm: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private nzMessageService: NzMessageService,
    private authorityService: AuthorityService
  ) { }

  ngOnInit() {
    this.searchForm = this.fb.group({
      name: [''],
      description: ['']
    });
    this.search();
  }

  search() {
    this.isSpinning = true;
    const model = new ApiResourceRequestModel();
    model.name = this.searchForm.get('name').value;
    model.description = this.searchForm.get('description').value;
    this.authorityService.selectApiResource(model).pipe(
      finalize(() => { this.isSpinning = false; })
    ).subscribe(
      result => {
        if (result.isSuccess) {
          this.apiResources = result.data;
          // this.apiResources = [...this.apiResources, result.Data[0]];
        } else {
          this.nzMessageService.error(result.message);
        }
      }
    );
  }

  reset() {
    /* this.searchForm.patchValue(
      {
        'Name': '',
        'Description': '',
      }
    ); */
    this.searchForm.reset();
    this.search();
  }

  new() {

  }

  edit() { }

  delete() { }
}
