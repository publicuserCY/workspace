import { Component, OnInit } from '@angular/core';
import { AuthorityService } from '../services/authority.service';
import { ApiResource } from '../models/api-resource';
import { ApiResourceRequestModel } from '../models/request';
import { finalize, delay } from 'rxjs/operators';
import { NzMessageService, NzModalRef } from 'ng-zorro-antd';
import { NzModalService } from 'ng-zorro-antd';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { OperationResult } from 'src/app/common/result';

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
    private nzModalService: NzModalService,
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
    this.searchForm.reset();
    this.search();
  }

  delete(id: number) {
    const modelRef: NzModalRef = this.nzModalService.confirm({
      nzTitle: '删除',
      nzContent: '<b>删除后不可恢复！</b>',
      nzOkText: '确认',
      nzOkType: 'danger',
      nzOnOk: () => new Promise((resolve, reject) => {
        const model = new ApiResourceRequestModel();
        model.id = id;
        this.authorityService.deleteApiResource(model).subscribe(
          result => { result.isSuccess ? resolve(result) : reject(result); });
      })
        .then((result: OperationResult<ApiResource>) => {
          /* const index = this.apiResources.findIndex(value => value.id === result.data.id);
          this.apiResources.splice(index, 1, result.data); */
          this.apiResources = this.apiResources.filter(p => p.id !== result.data.id);
          modelRef.destroy();
        })
        .catch((result: OperationResult<ApiResource>) => {
          this.nzMessageService.error(result.message);
          return false;
        }),
      nzCancelText: '取消',
      nzOnCancel: () => { }
    });
  }
}
