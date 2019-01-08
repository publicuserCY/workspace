import { Component, OnInit } from '@angular/core';
import { ApiResourceRequestModel } from '../models/api-resource-request.model';

@Component({
  selector: 'app-authority-clients',
  templateUrl: './clients.component.html',
  styleUrls: ['./clients.component.css']
})
export class ClientsComponent implements OnInit {
  apiResources: Array<ApiResourceRequestModel>;
  constructor() { }

  ngOnInit() {
  }
  search() {

  }
}
