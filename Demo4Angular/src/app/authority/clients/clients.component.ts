import { Component, OnInit } from '@angular/core';
import { AuthorityService } from '../services/authority.service';
import { ApiResourceRequestModel } from '../models/api-resource-request.model';

@Component({
  selector: 'app-authority-clients',
  templateUrl: './clients.component.html',
  styleUrls: ['./clients.component.css']
})
export class ClientsComponent implements OnInit {
  apiResources: Array<ApiResourceRequestModel>;
  constructor(private authorityService: AuthorityService) { }

  ngOnInit() {
  }
  search() {

  }
}
