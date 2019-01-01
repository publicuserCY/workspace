import { Component, OnInit } from '@angular/core';
import { AuthorityService } from '../services/authority.service';
import { ApiResourceRequestModel } from '../models/request';
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
    private nzMessageService: NzMessageService,
    private authorityService: AuthorityService
  ) { }

  ngOnInit() {
    this.search();
  }

  search() {
    this.isSpinning = true;
  }
}
