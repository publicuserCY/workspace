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
    const model = new ApiResourceRequestModel();
    this.authorityService.selectApiResource(model).subscribe(
      result => {
        console.log(result);
        if (result.isSuccess) {
          const data = result.data;
          data.forEach(p => {
            console.log(p.scopes);
          });
        } else {

        }
      }
    );
  }
}
