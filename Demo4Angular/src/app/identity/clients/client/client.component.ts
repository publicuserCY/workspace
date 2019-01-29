import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router, NavigationExtras } from '@angular/router';
import { finalize, map } from 'rxjs/operators';
import { NzModalService } from 'ng-zorro-antd';
import { NzMessageService, NzModalRef } from 'ng-zorro-antd';

import { Client } from '../../model/client.model';
import { ClientService } from '../../service/client.service';
import { ClientRequestModel } from '../../model/client-request.model';
import { OperationResult, PaginatedResult } from 'src/app/shared/result';
import { EntityState, Uris } from 'src/app/shared/const';

@Component({
  selector: 'app-identity-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.css']
})
export class ClientComponent implements OnInit {
  isSpinning = false;
  searchForm: FormGroup;
  paginatedresult = new PaginatedResult<Client>();
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
    private clientService: ClientService) { }

  ngOnInit() {
    this.searchForm = this.fb.group({
      enabled: [null],
      clientId: [null],
      clientName: [null]
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
    const model = new ClientRequestModel(Uris.RetrieveClient, this.paginatedresult.pageIndex, this.paginatedresult.pageSize, this.orderBy, this.direction);
    model.enabled = this.searchForm.get('enabled').value;
    model.clientId = this.searchForm.get('clientId').value;
    model.clientName = this.searchForm.get('clientName').value;
    this.clientService.retrieve(model).pipe(
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
        const client = this.paginatedresult.list.find(p => p.id === id);
        client.state = EntityState.Deleted;
        const model = new ClientRequestModel(Uris.DeleteClient);
        model.client = client;
        this.clientService.submit(model).subscribe(
          result => { result.isSuccess ? resolve(result) : reject(result); });
      })
        .then((result: OperationResult<Client>) => {
          this.paginatedresult.list = this.paginatedresult.list.filter(p => p.id !== result.data.id);
          this.nzMessageService.info('ApiResource 删除完成');
          modelRef.destroy();
        })
        .catch((result: OperationResult<Client>) => {
          this.nzMessageService.error(result.message);
          return false;
        }),
      nzCancelText: '取消',
      nzOnCancel: () => { modelRef.destroy(); }
    });
  }

  edit(id: number) {
    const navigationExtras: NavigationExtras = {
      relativeTo: this.route,
      queryParams: {
        'isEdit': true,
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

  enabledFilter(filterBy: boolean) {
    this.resetCondition();
    this.searchForm.patchValue({ enabled: filterBy });
    this.search();
  }

  clientNameFilter(filterBy: string) {
    this.resetCondition();
    this.searchForm.patchValue({ clientName: filterBy });
    this.search();
  }
}
