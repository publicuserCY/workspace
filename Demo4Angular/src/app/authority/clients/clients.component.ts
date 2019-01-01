import { Component, OnInit } from '@angular/core';
import { AuthorityService } from '../services/authority.service';
import { ApiResource } from '../models/api-resource';
import { ApiResourceRequestModel } from '../models/request';

@Component({
  selector: 'app-authority-clients',
  templateUrl: './clients.component.html',
  styleUrls: ['./clients.component.css']
})
export class ClientsComponent implements OnInit {
  apiResources: Array<ApiResource>;
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
