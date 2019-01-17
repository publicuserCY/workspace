import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router, NavigationExtras } from '@angular/router';
import { finalize, map } from 'rxjs/operators';
import { NzModalService } from 'ng-zorro-antd';
import { NzMessageService, NzModalRef } from 'ng-zorro-antd';

import { ApiResourceRequestModel } from '../model/api-resource-request.model';
import { ApiResource } from '../model/api-resource.model';
import { ApiResourceService } from '../service/api-resource.service';
import { OperationResult, PaginatedResult } from 'src/app/shared/result';
import { EntityState, Uris } from 'src/app/shared/const';

@Component({
  selector: 'app-authority-api-resource',
  templateUrl: './api-resource.component.html',
  styleUrls: ['./api-resource.component.css']
})
export class ApiResourceComponent implements OnInit {
  isSpinning = false;
  searchForm: FormGroup;
  paginatedresult = new PaginatedResult<ApiResource>();
  orderBy = 'created';
  direction = 'asc';
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
    /* this.route.paramMap.pipe(
      switchMap(params => {
        this.paginatedresult.pageSize = +params.get('pageSize');
        this.paginatedresult.pageIndex = +params.get('pageIndex');
        this.search();
        return of(empty);
      })
    ); */
    this.searchForm = this.fb.group({
      enabled: [null],
      name: [null],
      displayName: [null],
      description: [null]
    });
    this.route.queryParamMap
      .pipe(
        map(params => {
          if (params) {
            this.paginatedresult.pageIndex = params.get('pageIndex') ? +params.get('pageIndex') : 1;
          }
        }))
      .subscribe(
        () => { this.search(); }
      );
  }

  private search() {
    this.isSpinning = true;
    const model = new ApiResourceRequestModel(Uris.RetrieveApiResource, this.paginatedresult.pageIndex, this.paginatedresult.pageSize, this.orderBy, this.direction);
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

  retrieve() {
    this.paginatedresult.pageIndex = 1;
    this.orderBy = 'created';
    this.direction = 'asc';
    this.search();
  }

  reset() {
    this.resetCondition();
    this.search();
  }

  resetCondition() {
    this.searchForm.reset();
    this.paginatedresult.pageIndex = 1;
    this.orderBy = 'created';
    this.direction = 'asc';
  }

  delete(id: number) {
    const modelRef: NzModalRef = this.nzModalService.confirm({
      nzTitle: '删除',
      nzContent: '<b>删除后不可恢复！</b>',
      nzOkText: '确认',
      nzOkType: 'danger',
      nzOnOk: () => new Promise((resolve, reject) => {
        const apiResource = this.paginatedresult.list.find(p => p.id === id);
        apiResource.state = EntityState.Deleted;
        const model = new ApiResourceRequestModel(Uris.DeleteApiResource);
        model.apiResource = apiResource;
        this.apiResourceService.submit(model).subscribe(
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

  navigate(id: number) {
    const navigationExtras: NavigationExtras = {
      relativeTo: this.route,
      queryParams: {
        'pageIndex': this.paginatedresult.pageIndex
      }
    };
    this.router.navigate([id], navigationExtras);
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
