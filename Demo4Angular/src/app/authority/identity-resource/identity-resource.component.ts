import { Component, OnInit } from '@angular/core';
import { ApiResourceRequestModel } from '../models/api-resource-request.model';
import { finalize } from 'rxjs/operators';
import { NzMessageService } from 'ng-zorro-antd';
import { IdentityResource } from '../models/identity-resource';

@Component({
  selector: 'app-authority-identity-resource',
  templateUrl: './identity-resource.component.html',
  styleUrls: ['./identity-resource.component.css']
})
export class IdentityResourceComponent implements OnInit {
  isSpinning = false;
  identityResources: Array<IdentityResource>;

  constructor(
    private nzMessageService: NzMessageService
  ) { }

  ngOnInit() {
    this.search();
  }

  search() {
    this.isSpinning = true;
  }
}
