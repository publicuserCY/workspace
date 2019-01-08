import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize, delay } from 'rxjs/operators';
import { NzModalService } from 'ng-zorro-antd';
import { NzMessageService, NzModalRef } from 'ng-zorro-antd';

import { ApiResourceRequestModel } from '../models/api-resource-request.model';
import { ApiResource } from '../models/api-resource.model';
import { ApiResourceService } from '../services/api-resource.service';
import { OperationResult, PaginatedResult } from 'src/app/shared/result';

@Component({
  selector: 'app-authority-api-resource',
  templateUrl: './api-resource.component.html',
  styleUrls: ['./api-resource.component.css']
})
export class ApiResourceComponent implements OnInit {
  isSpinning = false;
  searchForm: FormGroup;
  paginatedresult = new PaginatedResult<ApiResource>();
  orderBy = null;
  direction = null;
  enabledCandidate = [
    { text: '启用', value: true },
    { text: '禁用', value: false }
  ];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private nzMessageService: NzMessageService,
    private nzModalService: NzModalService,
    private apiResourceService: ApiResourceService
  ) { }

  ngOnInit() {
    this.searchForm = this.fb.group({
      enabled: [null],
      name: [null],
      displayName: [null],
      description: [null]
    });
    this.search();
  }

  search() {
    this.isSpinning = true;
    const model = new ApiResourceRequestModel(this.paginatedresult.pageIndex, this.paginatedresult.pageSize, this.orderBy, this.direction);
    model.name = this.searchForm.get('name').value;
    model.description = this.searchForm.get('description').value;
    model.displayName = this.searchForm.get('displayName').value;
    model.enabled = this.searchForm.get('enabled').value;
    this.apiResourceService.retrieve(model).pipe(
      finalize(() => { this.isSpinning = false; })
    ).subscribe(
      result => {
        if (result.isSuccess) {
          this.paginatedresult = result.data;
        } else {
          this.nzMessageService.error(result.message);
        }
      }
    );
  }

  reset() {
    this.resetCondition();
    this.search();
  }

  resetCondition() {
    this.searchForm.reset();
    this.paginatedresult.pageIndex = 1;
    this.orderBy = '';
    this.direction = '';
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
        this.apiResourceService.delete(model).subscribe(
          result => { result.isSuccess ? resolve(result) : reject(result); });
      })
        .then((result: OperationResult<ApiResource>) => {
          this.paginatedresult.list = this.paginatedresult.list.filter(p => p.id !== result.data.id);
          this.nzMessageService.info('ApiResource 删除完成');
          modelRef.destroy();
        })
        .catch((result: OperationResult<ApiResource>) => {
          this.nzMessageService.error(result.message);
          return false;
        }),
      nzCancelText: '取消',
      nzOnCancel: () => { modelRef.destroy(); }
    });
  }

  sort(sort: { key: string, value: string }): void {
    this.orderBy = sort.key;
    this.direction = sort.value;
    this.search();
  }

  enabledFilter(enabled: boolean) {
    this.resetCondition();
    this.searchForm.patchValue({ 'enabled': enabled });
    this.search();
  }

  displayNameFilter(displayName: string) {
    this.resetCondition();
    this.searchForm.patchValue({ 'displayName': displayName });
    this.search();
  }
}
